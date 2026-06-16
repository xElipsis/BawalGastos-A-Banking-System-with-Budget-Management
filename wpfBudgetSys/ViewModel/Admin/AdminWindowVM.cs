using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.View;
using wpfBudgetSys.ViewModel.Admin.PanelViewModel;

namespace wpfBudgetSys.ViewModel.Admin
{
    public class AdminWindowVM : ViewModelBase
    {
        public const string NavDashboard = "Dashboard";
        public const string NavUsers = "Users";
        public const string NavAccounts = "Accounts";
        public const string NavTransactions = "Transactions";
        public const string NavNotifications = "Notifications";
        public const string NavReports = "Reports";

        private object currentView = null!;
        public object CurrentView
        {
            get => currentView;
            set { currentView = value; OnPropertyChanged(); }
        }

        private string viewTitle = "Admin Dashboard";
        public string ViewTitle
        {
            get => viewTitle;
            set { viewTitle = value; OnPropertyChanged(); }
        }

        private string selectedNav = NavDashboard;
        public string SelectedNav
        {
            get => selectedNav;
            set { selectedNav = value; OnPropertyChanged(); }
        }

        public Action? CloseAction { get; set; }

        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowUsersCommand { get; }
        public ICommand ShowAccountsCommand { get; }
        public ICommand ShowTransactionsCommand { get; }
        public ICommand ShowNotificationsCommand { get; }
        public ICommand ShowReportsCommand { get; }
        public ICommand LogoutCommand { get; }

        public AdminWindowVM()
        {
            ShowDashboardCommand = new RelayCommand(_ => Navigate(NavDashboard, "Admin Dashboard", () => new AdminDashboardPanelVM()));
            ShowUsersCommand = new RelayCommand(_ => Navigate(NavUsers, "User Management", () => new AdminUsersPanelVM()));
            ShowAccountsCommand = new RelayCommand(_ => Navigate(NavAccounts, "Account Management", () => new AdminAccountsPanelVM()));
            ShowTransactionsCommand = new RelayCommand(_ => Navigate(NavTransactions, "Transaction Oversight", () => new AdminTransactionsPanelVM()));
            ShowNotificationsCommand = new RelayCommand(_ => Navigate(NavNotifications, "Notifications", () => new AdminNotificationsPanelVM()));
            ShowReportsCommand = new RelayCommand(_ => Navigate(NavReports, "Reports", () => new AdminReportsPanelVM()));

            LogoutCommand = new RelayCommand(_ =>
            {
                SessionManager.Logout();
                new LoginPage().Show();
                CloseAction?.Invoke();
            });

            ShowDashboardCommand.Execute(null);
        }

        private void Navigate(string nav, string title, Func<object> createView)
        {
            SelectedNav = nav;
            ViewTitle = title;
            CurrentView = createView();
        }
    }
}
