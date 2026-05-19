using System.Windows.Input;
using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class DepositPanelVM : ViewModelBase
    {
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

        public ICommand DepositCommand { get; }

        public DepositPanelVM()
        {
            DepositCommand = new RelayCommand(_ =>
            {
                // TODO: wire to account/deposit service when available
            });
        }
    }
}
