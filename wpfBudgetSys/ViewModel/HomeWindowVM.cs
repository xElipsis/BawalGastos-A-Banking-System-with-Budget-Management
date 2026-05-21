using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Enums;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;
using wpfBudgetSys.View;
using wpfBudgetSys.ViewModel.Admin.PanelViewModel;
using wpfBudgetSys.ViewModel.PanelViewModel;

namespace wpfBudgetSys.ViewModel
{
    public class HomeWindowVM : ViewModelBase
    {
        public const string NavDashboard = "Dashboard";
        public const string NavPay = "Pay";
        public const string NavBudgets = "Budgets";
        public const string NavAlerts = "Alerts";
        public const string NavNotifications = "Notifications";
        public const string NavTransactions = "Transactions";
        public const string NavSettings = "Settings";
        public const string NavAdminUsers = "Users";
        public const string NavAdminAccounts = "Accounts";
        public const string NavAdminReports = "Reports";

        private readonly AccountServices accountServices = new();
        private readonly AlertService alertService = new();
        private readonly NotificationService notificationService = new();

        public bool IsAdmin { get; }
        public bool IsRegularUser => !IsAdmin;

        private object currentView = null!;
        public object CurrentView
        {
            get => currentView;
            set { currentView = value; OnPropertyChanged(); }
        }

        private string viewTitle = string.Empty;
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

        private bool showBackButton;
        public bool ShowBackButton
        {
            get => showBackButton;
            set { showBackButton = value; OnPropertyChanged(); }
        }

        private int unreadAlertCount;
        public int UnreadAlertCount
        {
            get => unreadAlertCount;
            set { unreadAlertCount = value; OnPropertyChanged(); }
        }

        private int unreadNotificationCount;
        public int UnreadNotificationCount
        {
            get => unreadNotificationCount;
            set { unreadNotificationCount = value; OnPropertyChanged(); }
        }

        public Action? CloseAction { get; set; }

        public ICommand ShowGetStartedCommand { get; }
        public ICommand ShowDashboardCommand { get; }
        public ICommand ShowDepositCommand { get; }
        public ICommand ShowWithdrawCommand { get; }
        public ICommand ShowTransferCommand { get; }
        public ICommand ShowPayCommand { get; }
        public ICommand ShowPayFromDashboardCommand { get; }
        public ICommand ShowBudgetsCommand { get; }
        public ICommand ShowAlertsCommand { get; }
        public ICommand ShowNotificationsCommand { get; }
        public ICommand ShowTransactionCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand ShowAdminUsersCommand { get; }
        public ICommand ShowAdminAccountsCommand { get; }
        public ICommand ShowAdminTransactionsCommand { get; }
        public ICommand ShowAdminNotificationsCommand { get; }
        public ICommand ShowAdminReportsCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand LogoutCommand { get; }

        public HomeWindowVM()
        {
            IsAdmin = SessionManager.CurrentUser?.Role == AppEnums.UserRole.Admin;

            if (!IsAdmin)
                NavBadgeNotifier.BadgesChanged += RefreshNavBadges;

            ShowDepositCommand = new RelayCommand(_ => NavigateFromDashboard(() =>
            {
                CurrentView = new DepositPanelVM();
                ViewTitle = "Deposit";
            }), _ => !IsAdmin);

            ShowWithdrawCommand = new RelayCommand(_ => NavigateFromDashboard(() =>
            {
                CurrentView = new WithdrawPanelVM();
                ViewTitle = "Withdraw";
            }), _ => !IsAdmin);

            ShowTransferCommand = new RelayCommand(_ => NavigateFromDashboard(() =>
            {
                CurrentView = new TransferPanelVM();
                ViewTitle = "Transfer";
            }), _ => !IsAdmin);

            ShowPayFromDashboardCommand = new RelayCommand(_ => NavigateFromDashboard(() =>
            {
                CurrentView = new PayPanelVM();
                ViewTitle = "Pay";
            }), _ => !IsAdmin);

            ShowPayCommand = new RelayCommand(_ => NavigateFromNav(NavPay, () =>
            {
                CurrentView = new PayPanelVM();
                ViewTitle = "Pay";
            }), _ => !IsAdmin);

            ShowBudgetsCommand = new RelayCommand(_ => NavigateFromNav(NavBudgets, () =>
            {
                CurrentView = new BudgetsPanelVM();
                ViewTitle = "Budgets";
            }), _ => !IsAdmin);

            ShowAlertsCommand = new RelayCommand(_ => NavigateFromNav(NavAlerts, () =>
            {
                CurrentView = new AlertsPanelVM();
                ViewTitle = "Alerts";
            }), _ => !IsAdmin);

            ShowNotificationsCommand = new RelayCommand(_ => NavigateFromNav(NavNotifications, () =>
            {
                CurrentView = IsAdmin ? new AdminNotificationsPanelVM() : new NotificationsPanelVM();
                ViewTitle = "Notifications";
            }));

            ShowTransactionCommand = new RelayCommand(_ => NavigateFromNav(NavTransactions, () =>
            {
                CurrentView = IsAdmin ? new AdminTransactionsPanelVM() : new TransactionPanelVM();
                ViewTitle = "Transactions";
            }));

            ShowSettingsCommand = new RelayCommand(_ => NavigateFromNav(NavSettings, () =>
            {
                CurrentView = new SettingsPanelVM();
                ViewTitle = "Settings";
            }), _ => !IsAdmin);

            ShowAdminUsersCommand = new RelayCommand(_ => NavigateFromNav(NavAdminUsers, () =>
            {
                CurrentView = new AdminUsersPanelVM();
                ViewTitle = "User Management";
            }), _ => IsAdmin);

            ShowAdminAccountsCommand = new RelayCommand(_ => NavigateFromNav(NavAdminAccounts, () =>
            {
                CurrentView = new AdminAccountsPanelVM();
                ViewTitle = "Account Management";
            }), _ => IsAdmin);

            ShowAdminTransactionsCommand = new RelayCommand(_ => NavigateFromNav(NavTransactions, () =>
            {
                CurrentView = new AdminTransactionsPanelVM();
                ViewTitle = "Transaction Oversight";
            }), _ => IsAdmin);

            ShowAdminNotificationsCommand = new RelayCommand(_ => NavigateFromNav(NavNotifications, () =>
            {
                CurrentView = new AdminNotificationsPanelVM();
                ViewTitle = "Send Notifications";
            }), _ => IsAdmin);

            ShowAdminReportsCommand = new RelayCommand(_ => NavigateFromNav(NavAdminReports, () =>
            {
                CurrentView = new AdminReportsPanelVM();
                ViewTitle = "Reports";
            }), _ => IsAdmin);

            ShowDashboardCommand = new RelayCommand(_ =>
            {
                ShowBackButton = false;
                SelectedNav = NavDashboard;
                if (IsAdmin)
                {
                    CurrentView = new AdminDashboardPanelVM();
                    ViewTitle = "Admin Dashboard";
                }
                else
                {
                    CurrentView = CreateDashboardView();
                    ViewTitle = "Dashboard";
                }
            });

            GoBackCommand = new RelayCommand(_ => ShowDashboardCommand.Execute(null), _ => !IsAdmin);

            ShowGetStartedCommand = new RelayCommand(_ => NavigateFromNav(NavDashboard, () =>
            {
                var getStartedVm = new GetStartedPanelVM();
                getStartedVm.OnSetupComplete = () => ShowDashboardCommand.Execute(null);
                CurrentView = getStartedVm;
                ViewTitle = "Get Started";
            }), _ => !IsAdmin);

            LogoutCommand = new RelayCommand(_ => Logout());

            NavigateToInitialView();
            if (!IsAdmin)
                RefreshNavBadges();
        }

        public void NavigateToInitialView()
        {
            if (IsAdmin)
            {
                ShowDashboardCommand.Execute(null);
                return;
            }

            if (accountServices.CurrentUserAccountIsClosed())
            {
                ShowBackButton = false;
                SelectedNav = NavDashboard;
                CurrentView = new AccountClosedPanelVM();
                ViewTitle = "Account Closed";
                return;
            }

            if (accountServices.CurrentUserHasActiveAccount())
                ShowDashboardCommand.Execute(null);
            else if (accountServices.CurrentUserHasAccount())
                ShowDashboardCommand.Execute(null);
            else
                ShowGetStartedCommand.Execute(null);
        }

        private void NavigateFromDashboard(Action navigate)
        {
            ShowBackButton = true;
            navigate();
        }

        private void NavigateFromNav(string navKey, Action navigate)
        {
            ShowBackButton = false;
            SelectedNav = navKey;
            navigate();
        }

        private void RefreshNavBadges()
        {
            UnreadAlertCount = alertService.GetUnreadCountForCurrentUser();
            UnreadNotificationCount = notificationService.GetUnreadCountForCurrentUser();
        }

        private void Logout()
        {
            if (!IsAdmin)
                NavBadgeNotifier.BadgesChanged -= RefreshNavBadges;

            SessionManager.Logout();
            new LoginPage().Show();
            CloseAction?.Invoke();
        }

        private HomeWindowPanelVM CreateDashboardView() =>
            new HomeWindowPanelVM(
                ShowDepositCommand,
                ShowWithdrawCommand,
                ShowTransferCommand,
                ShowPayFromDashboardCommand);
    }
}
