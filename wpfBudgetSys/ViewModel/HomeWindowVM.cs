using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;
using wpfBudgetSys.View;
using wpfBudgetSys.ViewModel.PanelViewModel;

namespace wpfBudgetSys.ViewModel
{
    public class HomeWindowVM : ViewModelBase
    {
        private readonly AccountServices accountServices = new();

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

        private bool showBackButton;
        public bool ShowBackButton
        {
            get => showBackButton;
            set { showBackButton = value; OnPropertyChanged(); }
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
        public ICommand ShowTransactionCommand { get; }
        public ICommand ShowSettingsCommand { get; }
        public ICommand GoBackCommand { get; }
        public ICommand LogoutCommand { get; }

        public HomeWindowVM()
        {
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

            ShowPayCommand = new RelayCommand(_ => NavigateFromNav(() =>
            {
                CurrentView = new PayPanelVM();
                ViewTitle = "Pay";
            }));

            ShowBudgetsCommand = new RelayCommand(_ => NavigateFromNav(() =>
            {
                CurrentView = new BudgetsPanelVM();
                ViewTitle = "Budgets";
            }));

            ShowAlertsCommand = new RelayCommand(_ => NavigateFromNav(() =>
            {
                CurrentView = new AlertsPanelVM();
                ViewTitle = "Alerts";
            }));

            ShowTransactionCommand = new RelayCommand(_ => NavigateFromNav(() =>
            {
                CurrentView = new TransactionPanelVM();
                ViewTitle = "Transactions";
            }));

            ShowSettingsCommand = new RelayCommand(_ => NavigateFromNav(() =>
            {
                CurrentView = new SettingsPanelVM();
                ViewTitle = "Settings";
            }));

            ShowDashboardCommand = new RelayCommand(_ =>
            {
                ShowBackButton = false;
                CurrentView = CreateDashboardView();
                ViewTitle = "Dashboard";
            });

            GoBackCommand = new RelayCommand(_ => ShowDashboardCommand.Execute(null));

            ShowGetStartedCommand = new RelayCommand(_ => NavigateFromNav(() =>
            {
                var getStartedVm = new GetStartedPanelVM();
                getStartedVm.OnSetupComplete = () => ShowDashboardCommand.Execute(null);
                CurrentView = getStartedVm;
                ViewTitle = "Get Started";
            }));

            LogoutCommand = new RelayCommand(_ => Logout());

            NavigateToInitialView();
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

        private void NavigateFromNav(Action navigate)
        {
            ShowBackButton = false;
            navigate();
        }

        private void Logout()
        {
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
