using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class BudgetService
    {
        private readonly SpendingLimitsRepository spendingLimitsRepository = new();
        private readonly ExpenseCategoryRepository expenseCategoryRepository = new();

        public List<BudgetLimitRow> GetBudgetsForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return new List<BudgetLimitRow>();

            return spendingLimitsRepository.GetBudgetRowsByUserId(SessionManager.CurrentUser.UserId);
        }

        public List<ExpenseCategory> GetAvailablePresetsForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return new List<ExpenseCategory>();

            int userId = SessionManager.CurrentUser.UserId;
            var presets = expenseCategoryRepository.GetDefaultCategories();
            var existing = spendingLimitsRepository.GetBudgetRowsByUserId(userId)
                .Select(r => r.CategoryId)
                .ToHashSet();

            var available = presets
                .Where(p => !existing.Contains(p.CategoryId))
                .ToList();

            available.Add(new ExpenseCategory
            {
                CategoryId = 0,
                CategoryName = "+ Custom",
                IsDefault = false
            });

            return available;
        }

        public string? AddBudget(int categoryId, string? customCategoryName, decimal monthlyLimit, decimal dailyLimit)
        {
            if (SessionManager.CurrentUser == null)
                return "You must be logged in.";

            if (monthlyLimit < 0 || dailyLimit < 0)
                return "Limits cannot be negative.";

            int userId = SessionManager.CurrentUser.UserId;

            try
            {
                using MySqlConnection conn = DBConnection.GetConnection();
                conn.Open();
                using MySqlTransaction tx = conn.BeginTransaction();

                int resolvedCategoryId;

                if (categoryId == 0)
                {
                    if (string.IsNullOrWhiteSpace(customCategoryName))
                        return "Enter a name for the custom category.";

                    resolvedCategoryId = expenseCategoryRepository.InsertCustomCategory(
                        new ExpenseCategory
                        {
                            UserId = userId,
                            CategoryName = customCategoryName.Trim(),
                            IsDefault = false
                        }, conn, tx);
                }
                else
                {
                    if (spendingLimitsRepository.ExistsForUserCategory(userId, categoryId))
                    {
                        tx.Rollback();
                        return "This category is already in your budget.";
                    }

                    resolvedCategoryId = categoryId;
                }

                spendingLimitsRepository.Insert(new SpendingLimits
                {
                    UserId = userId,
                    CategoryId = resolvedCategoryId,
                    MonthlyLimit = monthlyLimit,
                    DailyLimit = dailyLimit
                }, conn, tx);

                tx.Commit();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string? UpdateBudget(int limitId, decimal monthlyLimit, decimal dailyLimit)
        {
            if (SessionManager.CurrentUser == null)
                return "You must be logged in.";

            if (monthlyLimit < 0 || dailyLimit < 0)
                return "Limits cannot be negative.";

            var existing = spendingLimitsRepository.GetByLimitId(limitId, SessionManager.CurrentUser.UserId);
            if (existing == null)
                return "Budget entry not found.";

            existing.MonthlyLimit = monthlyLimit;
            existing.DailyLimit = dailyLimit;
            spendingLimitsRepository.Update(existing);
            return null;
        }

        public string? DeleteBudget(int limitId)
        {
            if (SessionManager.CurrentUser == null)
                return "You must be logged in.";

            var existing = spendingLimitsRepository.GetByLimitId(limitId, SessionManager.CurrentUser.UserId);
            if (existing == null)
                return "Budget entry not found.";

            spendingLimitsRepository.Delete(limitId, SessionManager.CurrentUser.UserId);
            return null;
        }
    }
}
