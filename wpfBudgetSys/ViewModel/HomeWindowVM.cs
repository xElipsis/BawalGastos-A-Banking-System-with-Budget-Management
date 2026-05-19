using System.Windows.Input;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.ViewModel.PanelViewModel;

namespace wpfBudgetSys.ViewModel
{
    public class HomeWindowVM : ViewModelBase
    {
        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set 
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        private string viewTitle;
        public string ViewTitle
        {
            get { return viewTitle; }
            set
            {
                viewTitle = value;
                OnPropertyChanged();
            }
        }

        public ICommand ShowGetStartedCommand { get; set; }
        public ICommand ShowDashboardCommand { get; set; }
        public ICommand ShowDepositCommand { get; set; }
        public ICommand ShowWithdrawCommand { get; set; }
        public ICommand ShowTransferCommand { get; set; }
        public ICommand ShowPayCommand { get; set; }
        public ICommand ShowTransactionCommand { get; set; }
        public ICommand ShowSettingsCommand { get; set; }

        public HomeWindowVM()
        {
            ShowGetStartedCommand = new RelayCommand(o =>
            {
                CurrentView = new GetStartedPanelVM();
                ViewTitle = "Get Started";
            });

            ShowDashboardCommand = new RelayCommand(o =>
            {
                CurrentView = new HomeWindowPanelVM();
                ViewTitle = "Dashboard";
            });

            ShowDepositCommand = new RelayCommand(o =>
            {
                CurrentView = new DepositPanelVM();
                ViewTitle = "Deposit";
            });

            ShowWithdrawCommand = new RelayCommand(o =>
            {
                CurrentView = new WithdrawPanelVM();
                ViewTitle = "Withdraw";
            });

            ShowTransferCommand = new RelayCommand(o =>
            {
                CurrentView = new TransferPanelVM();
                ViewTitle = "Transfer";
            });

            ShowPayCommand = new RelayCommand(o =>
            {
                CurrentView = new PayPanelVM();
                ViewTitle = "Pay";
            });

            ShowTransactionCommand = new RelayCommand(o =>
            {
                CurrentView = new TransactionPanelVM();
                ViewTitle = "Transaction";
            });

            ShowSettingsCommand = new RelayCommand(o =>
            {
                CurrentView = new SettingsPanelVM();
                ViewTitle = "Settings";
            });

            ShowGetStartedCommand.Execute(null);
        }
    }
}
