using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class AccountSetupServices
    {
        private readonly AccountRepository accountRepository = new();
        private readonly ExpenseCategoryRepository expenseCategoryRepository = new();
        private readonly SpendingLimitsRepository spendingLimitsRepository = new();

        public AccountSetupResult CompleteSetup(AccountSetupRequest request)
        {
            var validationError = ValidateRequest(request);
            if (validationError != null)
                return Fail(validationError);

            if (SessionManager.CurrentUser == null)
                return Fail("You must be logged in to create an account.");

            try
            {
                using MySqlConnection conn = DBConnection.GetConnection();
                conn.Open();
                using MySqlTransaction transaction = conn.BeginTransaction();

                try
                {
                    if (accountRepository.UserHasAccount(request.UserId, conn, transaction))
                    {
                        transaction.Rollback();
                        return Fail("You already have an account set up.");
                    }

                    if (accountRepository.AccountNumberExists(request.AccountNumber, conn, transaction))
                    {
                        transaction.Rollback();
                        return Fail("That account number is already in use. Please generate a new one.");
                    }

                    var account = new Account
                    {
                        UserId = request.UserId,
                        AccountNumber = request.AccountNumber,
                        AccountType = request.AccountType,
                        Balance = request.InitialDeposit,
                        Status = "Active"
                    };

                    int accountId = accountRepository.Insert(account, conn, transaction);

                    foreach (BudgetCategorySetupItem item in request.Categories)
                    {
                        int categoryId = ResolveCategoryId(item, request.UserId, conn, transaction, expenseCategoryRepository);

                        spendingLimitsRepository.Insert(new SpendingLimits
                        {
                            UserId = request.UserId,
                            CategoryId = categoryId,
                            MonthlyLimit = item.MonthlyLimit,
                            DailyLimit = item.DailyLimit
                        }, conn, transaction);
                    }

                    transaction.Commit();
                    return new AccountSetupResult
                    {
                        Success = true,
                        AccountId = accountId,
                        Message = "Account created successfully."
                    };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            catch (Exception ex)
            {
                return Fail($"Could not complete account setup: {ex.Message}");
            }
        }

        private static int ResolveCategoryId(
            BudgetCategorySetupItem item,
            int userId,
            MySqlConnection conn,
            MySqlTransaction transaction,
            ExpenseCategoryRepository expenseCategoryRepository)
        {
            if (item.IsDefault)
            {
                if (item.CategoryId <= 0)
                    throw new InvalidOperationException("Preset category is missing a valid category id.");

                return item.CategoryId;
            }

            if (string.IsNullOrWhiteSpace(item.CategoryName))
                throw new InvalidOperationException("Custom category name is required.");

            return expenseCategoryRepository.InsertCustomCategory(new ExpenseCategory
            {
                UserId = userId,
                CategoryName = item.CategoryName.Trim(),
                IsDefault = false
            }, conn, transaction);
        }

        private static string? ValidateRequest(AccountSetupRequest request)
        {
            if (request.UserId <= 0)
                return "Invalid user.";

            if (string.IsNullOrWhiteSpace(request.AccountNumber))
                return "Account number is required.";

            if (string.IsNullOrWhiteSpace(request.AccountType))
                return "Account type is required.";

            if (request.AccountType is not "Savings" and not "Checking")
                return "Account type must be Savings or Checking.";

            if (request.InitialDeposit < 0)
                return "Initial deposit cannot be negative.";

            if (request.Categories == null || request.Categories.Count == 0)
                return "Add at least one budget category.";

            var presetIds = new HashSet<int>();
            var customNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (BudgetCategorySetupItem item in request.Categories)
            {
                if (item.MonthlyLimit < 0 || item.DailyLimit < 0)
                    return "Spending limits cannot be negative.";

                if (item.IsDefault)
                {
                    if (item.CategoryId <= 0)
                        return "A preset category is invalid.";

                    if (!presetIds.Add(item.CategoryId))
                        return "Each preset category can only be added once.";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(item.CategoryName))
                        return "Enter a name for each custom category.";

                    if (!customNames.Add(item.CategoryName.Trim()))
                        return "Custom category names must be unique.";
                }
            }

            return null;
        }

        private static AccountSetupResult Fail(string message) =>
            new() { Success = false, Message = message };
    }
}
