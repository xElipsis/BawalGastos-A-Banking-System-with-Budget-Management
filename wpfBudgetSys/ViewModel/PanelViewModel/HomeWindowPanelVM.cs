using System.Windows.Input;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class HomeWindowPanelVM : ViewModelBase
    {
        private readonly AccountServices accountService = new();

        public ICommand ShowDepositCommand { get; }
        public ICommand ShowWithdrawCommand { get; }
        public ICommand ShowTransferCommand { get; }
        public ICommand ShowPayCommand { get; }

        public string GreetingName => SessionManager.CurrentUser?.Fullname ?? "there";

        private string formattedBalance = "₱0.00";
        public string FormattedBalance
        {
            get => formattedBalance;
            private set { formattedBalance = value; OnPropertyChanged(); }
        }

        private string accountNumber = "—";
        public string AccountNumber
        {
            get => accountNumber;
            private set { accountNumber = value; OnPropertyChanged(); }
        }

        private string accountType = "—";
        public string AccountType
        {
            get => accountType;
            private set { accountType = value; OnPropertyChanged(); }
        }

        private string accountStatus = "—";
        public string AccountStatus
        {
            get => accountStatus;
            private set { accountStatus = value; OnPropertyChanged(); }
        }

        private int budgetCategoryCount;
        public int BudgetCategoryCount
        {
            get => budgetCategoryCount;
            private set
            {
                budgetCategoryCount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(BudgetCategorySummary));
            }
        }

        public string BudgetCategorySummary =>
            budgetCategoryCount == 1
                ? "1 budget category"
                : $"{budgetCategoryCount} budget categories";

        public bool HasAccount { get; private set; }

        public HomeWindowPanelVM(
            ICommand showDepositCommand,
            ICommand showWithdrawCommand,
            ICommand showTransferCommand,
            ICommand showPayCommand)
        {
            ShowDepositCommand = showDepositCommand;
            ShowWithdrawCommand = showWithdrawCommand;
            ShowTransferCommand = showTransferCommand;
            ShowPayCommand = showPayCommand;

            LoadDashboard();
        }

        public void Refresh()
        {
            LoadDashboard();
        }

        private void LoadDashboard()
        {
            Account? account = accountService.GetAccountForCurrentUser();
            HasAccount = account != null;

            if (account == null)
            {
                FormattedBalance = "₱0.00";
                AccountNumber = "—";
                AccountType = "—";
                AccountStatus = "—";
                BudgetCategoryCount = 0;
                return;
            }

            FormattedBalance = $"₱{account.Balance:N2}";
            AccountNumber = account.AccountNumber;
            AccountType = account.AccountType;
            AccountStatus = account.Status;
            BudgetCategoryCount = accountService.GetBudgetCategoryCountForCurrentUser();
        }
    }
}
