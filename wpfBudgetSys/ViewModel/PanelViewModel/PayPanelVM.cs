using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class PayPanelVM : ViewModelBase
    {
        private readonly TransactionService transactionServices = new();
        private readonly AccountServices accountServices = new();
        private readonly ExpenseCategoryServices categoryServices = new();

        public List<ExpenseCategory> Categories { get; }

        private string account = string.Empty;
        public string Account
        {
            get => account;
            set { account = value; OnPropertyChanged(); }
        }

        private ExpenseCategory? selectedCategory;
        public ExpenseCategory? SelectedCategory
        {
            get => selectedCategory;
            set { selectedCategory = value; OnPropertyChanged(); }
        }

        private string payee = string.Empty;
        public string Payee
        {
            get => payee;
            set { payee = value; OnPropertyChanged(); }
        }

        private string amount = string.Empty;
        public string Amount
        {
            get => amount;
            set { amount = value; OnPropertyChanged(); }
        }

        private string statusMessage = string.Empty;
        public string StatusMessage
        {
            get => statusMessage;
            set { statusMessage = value; OnPropertyChanged(); }
        }

        public ICommand PayCommand { get; }

        public PayPanelVM()
        {
            var userAccount = accountServices.GetAccountForCurrentUser();
            if (userAccount != null)
                Account = userAccount.AccountNumber;

            int userId = SessionManager.CurrentUser?.UserId ?? 0;
            Categories = userId > 0 ? categoryServices.GetUserBudgetCategories(userId) : new List<ExpenseCategory>();

            if (Categories.Count > 0)
                SelectedCategory = Categories[0];

            PayCommand = new RelayCommand(_ => ExecutePay());
        }

        public void ExecutePay()
        {
            StatusMessage = string.Empty;

            if (SessionManager.CurrentUser == null)
            {
                StatusMessage = "You must be logged in.";
                return;
            }

            if (string.IsNullOrWhiteSpace(Account))
            {
                StatusMessage = "Enter an account number.";
                return;
            }

            if (!TryParseAmount(Amount, out decimal payAmount))
            {
                StatusMessage = "Enter a valid payment amount greater than zero.";
                return;
            }

            var accountRecord = accountServices.GetAccountByAccountNumber(Account.Trim());
            if (accountRecord == null)
            {
                StatusMessage = "Account number not found.";
                return;
            }

            if (accountRecord.UserId != SessionManager.CurrentUser.UserId)
            {
                StatusMessage = "You can only pay from your own account.";
                return;
            }

            if (accountRecord.Balance < payAmount)
            {
                StatusMessage = "Insufficient balance for this payment.";
                return;
            }

            if (accountRecord.Status == "Frozen")
            {
                StatusMessage = "This account is frozen. You cannot deposit into a frozen account.";
                return;
            }

            if (SelectedCategory == null)
            {
                StatusMessage = "Select a budget category.";
                return;
            }

            try
            {
                string referenceNumber = transactionServices.Pay(accountRecord.AccountId, SelectedCategory, Payee, payAmount);

                string message = $"Paid ₱{payAmount:N2} successfully.";

                AppDialog.Show(
                    $"Paid ₱{payAmount:N2} successfully.",
                    "Payment Successful",
                    AppDialogIcon.Success);

                string body = EmailTemplates.PaymentConfirmation(accountRecord.AccountNumber, SelectedCategory.CategoryName, Payee, payAmount, referenceNumber);

                EmailHelper.SendEmail(
                    SessionManager.CurrentUser.Email,
                    "Payment Confirmation",
                    body);

                Amount = string.Empty;
                Payee = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
        }

        private static bool TryParseAmount(string value, out decimal amount)
        {
            if (decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out amount) && amount > 0)
                return true;

            return decimal.TryParse(value, out amount) && amount > 0;
        }
    }
}
