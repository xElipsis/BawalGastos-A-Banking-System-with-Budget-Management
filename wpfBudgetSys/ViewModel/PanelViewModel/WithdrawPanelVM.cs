using System.Globalization;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class WithdrawPanelVM : ViewModelBase
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

        public ICommand WithdrawCommand { get; }

        public WithdrawPanelVM()
        {
            var userAccount = accountServices.GetAccountForCurrentUser();
            if (userAccount != null)
                Account = userAccount.AccountNumber;

            WithdrawCommand = new RelayCommand(_ => ExecuteWithdraw());
        }

        private void ExecuteWithdraw()
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

            if (!TryParseAmount(Amount, out decimal withdrawAmount))
            {
                StatusMessage = "Enter a valid withdrawal amount greater than zero.";
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
                StatusMessage = "You can only withdraw from your own account.";
                return;
            }

            if (accountRecord.Balance < withdrawAmount)
            {
                StatusMessage = "Insufficient balance for this withdrawal.";
                return;
            }

            if (accountRecord.Status == "Frozen")
            {
                StatusMessage = "This account is frozen. You cannot deposit into a frozen account.";
                return;
            }

            try
            {
                string referenceNumber = transactionServices.Withdraw(accountRecord.AccountId, withdrawAmount);

                AppDialog.Show(
                    $"Withdrew ₱{withdrawAmount:N2} successfully.",
                    "Withdraw Successful",
                    AppDialogIcon.Success);

                string body = EmailTemplates.WithdrawalConfirmation(accountRecord.AccountNumber, withdrawAmount, referenceNumber);

                EmailHelper.SendEmail(
                    SessionManager.CurrentUser.Email,
                    "Withdrawal Confirmation",
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
