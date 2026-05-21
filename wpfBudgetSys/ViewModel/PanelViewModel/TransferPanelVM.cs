using System.Globalization;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class TransferPanelVM : ViewModelBase
    {
        private readonly TransactionService transactionServices = new();
        private readonly AccountServices accountServices = new();

        private string fromAccount = string.Empty;
        public string FromAccount
        {
            get => fromAccount;
            set { fromAccount = value; OnPropertyChanged(); }
        }

        private string toAccount = string.Empty;
        public string ToAccount
        {
            get => toAccount;
            set { toAccount = value; OnPropertyChanged(); }
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

        public ICommand TransferCommand { get; set; }

        public TransferPanelVM()
        {
            var userAccount = accountServices.GetAccountForCurrentUser();
            if (userAccount != null)
                FromAccount = userAccount.AccountNumber;

            TransferCommand = new RelayCommand(_ => ExecuteTransfer());
        }

        private void ExecuteTransfer()
        {
            StatusMessage = string.Empty;

            if (SessionManager.CurrentUser == null)
            {
                StatusMessage = "You must be logged in.";
                return;
            }
            if (string.IsNullOrWhiteSpace(FromAccount) || string.IsNullOrWhiteSpace(ToAccount))
            {
                StatusMessage = "Enter both account numbers.";
                return;
            }

            if (!TryParseAmount(Amount, out decimal transferAmount))
            {
                StatusMessage = "Enter a valid transfer amount greater than zero.";
                return;
            }

            var fromAccountRecord = accountServices.GetAccountByAccountNumber(FromAccount.Trim());
            var toAccountRecord = accountServices.GetAccountByAccountNumber(ToAccount.Trim());
            if (fromAccountRecord == null)
            {
                StatusMessage = "Account number not found.";
                return;
            }

            if (toAccountRecord == null)
            {
                StatusMessage = "Destination Account number not found.";
                return;
            }

            if (fromAccountRecord == toAccountRecord)
            {
                StatusMessage = "Cannot transfer to the same account.";
                return;
            }

            if (fromAccountRecord.Status == "Frozen")
            {
                StatusMessage = "This account is frozen. You cannot transfer from a frozen account.";
                return;
            }

            if (toAccountRecord.Status == "Frozen")
            {
                StatusMessage = "This account is frozen. You cannot transfer to a frozen account.";
                return;
            }
            try
            {
                string referenceNumber = transactionServices.Transfer(fromAccountRecord, toAccountRecord, transferAmount);

                AppDialog.Show(
                    $"Transfered ₱{transferAmount:N2} successfully.",
                    "Transfer Successful",
                    AppDialogIcon.Success);

                string body = EmailTemplates.TransferConfirmation(fromAccountRecord.AccountNumber, toAccountRecord.AccountNumber, transferAmount, referenceNumber);

                EmailHelper.SendEmail(
                    SessionManager.CurrentUser.Email,
                    "Transfer Confirmation",
                    body);

                body = EmailTemplates.ReceivedTransferNotification(fromAccountRecord.AccountNumber, toAccountRecord.AccountNumber, transferAmount, referenceNumber);

                EmailHelper.SendEmail(
                    accountServices.GetEmailByUserId(toAccountRecord.UserId),
                    "Transfer Received",
                    body);

                Amount = string.Empty;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Transfer failed: {ex.Message}";
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
