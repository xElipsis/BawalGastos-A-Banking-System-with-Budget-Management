using System.Globalization;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class DepositPanelVM : ViewModelBase
    {
        private readonly TransactionService transactionServices = new();
        private readonly AccountServices accountServices = new();

        private string account = string.Empty;
        public string Account
        {
            get => account;
            set { account = value; OnPropertyChanged(); }
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

        public ICommand DepositCommand { get; }

        public DepositPanelVM()
        {
            var userAccount = accountServices.GetAccountForCurrentUser();
            if (userAccount != null)
                Account = userAccount.AccountNumber;

            DepositCommand = new RelayCommand(_ => ExecuteDeposit());
        }

        private void ExecuteDeposit()
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

            if (!TryParseAmount(Amount, out decimal depositAmount))
            {
                StatusMessage = "Enter a valid deposit amount greater than zero.";
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
                StatusMessage = "You can only deposit into your own account.";
                return;
            }

            if (accountRecord.Status == "Frozen")
            {
                StatusMessage = "This account is frozen. You cannot deposit into a frozen account.";
                return;
            }

            try
            {
                var referenceNumber = transactionServices.Deposit(accountRecord.AccountId, depositAmount);

                AppDialog.Show(
                    $"Deposited ₱{depositAmount:N2} successfully.",
                    "Deposit Successful",
                    AppDialogIcon.Success);

                string body = EmailTemplates.DepositConfirmation(accountRecord.AccountNumber, depositAmount, referenceNumber);

                EmailHelper.SendEmail(
                    SessionManager.CurrentUser.Email,
                    "Deposit Confirmation",
                    body);

                Amount = string.Empty;
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
