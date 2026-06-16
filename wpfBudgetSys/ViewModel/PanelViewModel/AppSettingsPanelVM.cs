using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class AppSettingsPanelVM : ViewModelBase
    {
        public ICommand BackCommand { get; }
        public ICommand SaveCommand { get; }

        public bool ShowBalanceOnDashboard
        {
            get => AppSettingsStore.ShowBalanceOnDashboard;
            set { AppSettingsStore.ShowBalanceOnDashboard = value; OnPropertyChanged(); }
        }

        public bool EnableBudgetAlertEmails
        {
            get => AppSettingsStore.EnableBudgetAlertEmails;
            set { AppSettingsStore.EnableBudgetAlertEmails = value; OnPropertyChanged(); }
        }

        public bool CompactTransactionList
        {
            get => AppSettingsStore.CompactTransactionList;
            set { AppSettingsStore.CompactTransactionList = value; OnPropertyChanged(); }
        }

        public AppSettingsPanelVM(ICommand backCommand)
        {
            BackCommand = backCommand;
            SaveCommand = new RelayCommand(_ =>
                MessageBox.Show("App settings saved.", "App settings", MessageBoxButton.OK, MessageBoxImage.Information));
        }
    }
}
