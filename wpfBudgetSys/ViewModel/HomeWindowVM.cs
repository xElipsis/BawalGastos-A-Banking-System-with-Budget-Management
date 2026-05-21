using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;
using wpfBudgetSys.View;
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

        private readonly AccountServices accountServices = new();
        private readonly AlertService alertService = new();
        private readonly NotificationService notificationService = new();

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

        private bool hasUnreadAlerts;
        public bool HasUnreadAlerts
        {
            get => hasUnreadAlerts;
            set { hasUnreadAlerts = value; OnPropertyChanged(); }
        }

        private bool hasUnreadNotifications;
        public bool HasUnreadNotifications
        {
            get => hasUnreadNotifications;
            set { hasUnreadNotifications = value; OnPropertyChanged(); }
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
        public ICommand GoBackCommand { get; }
        public ICommand LogoutCommand { get; }

        public HomeWindowVM()
        {
            NavBadgeNotifier.BadgesChanged += RefreshNavBadges;

            ShowDepositCommand = new RelayCommand(_ => NavigateFromDashboard(() =>
            {
                CurrentView = new DepositPanelVM();
                ViewTitle = "Deposit";
            }));

            ShowWithdrawCommand = new RelayCommand(_ => NavigateFromDashboard(() =>
            {
                CurrentView = new WithdrawPanelVM();
                ViewTitle = "Withdraw";
            }));

            ShowTransferCommand = new RelayCommand(_ => NavigateFromDashboard(() =>
            {
                CurrentView = new TransferPanelVM();
                ViewTitle = "Transfer";
            }));

            ShowPayFromDashboardCommand = new RelayCommand(_ => NavigateFromDashboard(() =>
            {
                CurrentView = new PayPanelVM();
                ViewTitle = "Pay";
            }));

            ShowPayCommand = new RelayCommand(_ => NavigateFromNav(NavPay, () =>
            {
                CurrentView = new PayPanelVM();
                ViewTitle = "Pay";
            }));

            ShowBudgetsCommand = new RelayCommand(_ => NavigateFromNav(NavBudgets, () =>
            {
                CurrentView = new BudgetsPanelVM();
                ViewTitle = "Budgets";
            }));

            ShowAlertsCommand = new RelayCommand(_ => NavigateFromNav(NavAlerts, () =>
            {
                CurrentView = new AlertsPanelVM();
                ViewTitle = "Alerts";
            }));

            ShowNotificationsCommand = new RelayCommand(_ => NavigateFromNav(NavNotifications, () =>
            {
                CurrentView = new NotificationsPanelVM();
                ViewTitle = "Notifications";
            }));

            ShowTransactionCommand = new RelayCommand(_ => NavigateFromNav(NavTransactions, () =>
            {
                CurrentView = new TransactionPanelVM();
                ViewTitle = "Transactions";
            }));

            ShowSettingsCommand = new RelayCommand(_ => NavigateFromNav(NavSettings, () =>
            {
                CurrentView = new SettingsPanelVM();
                ViewTitle = "Settings";
            }));

            ShowDashboardCommand = new RelayCommand(_ =>
            {
                ShowBackButton = false;
                SelectedNav = NavDashboard;
                CurrentView = CreateDashboardView();
                ViewTitle = "Dashboard";
            });

            GoBackCommand = new RelayCommand(_ => ShowDashboardCommand.Execute(null));

            ShowGetStartedCommand = new RelayCommand(_ => NavigateFromNav(NavDashboard, () =>
            {
                var getStartedVm = new GetStartedPanelVM();
                getStartedVm.OnSetupComplete = () => ShowDashboardCommand.Execute(null);
                CurrentView = getStartedVm;
                ViewTitle = "Get Started";
            }));

            LogoutCommand = new RelayCommand(_ => Logout());

            NavigateToInitialView();
            RefreshNavBadges();
        }

        public void NavigateToInitialView()
        {
            if (accountServices.CurrentUserHasAccount())
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
            HasUnreadAlerts = alertService.GetUnreadCountForCurrentUser() > 0;
            HasUnreadNotifications = notificationService.GetUnreadCountForCurrentUser() > 0;
        }

        private void Logout()
        {
            NavBadgeNotifier.BadgesChanged -= RefreshNavBadges;
            SessionManager.Logout();
            var loginWindow = new LoginPage();
            loginWindow.Show();
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
