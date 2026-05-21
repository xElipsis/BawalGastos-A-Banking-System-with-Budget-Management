using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.Admin.PanelViewModel
{
    public class AdminReportsPanelVM : ViewModelBase
    {
        private readonly AdminService adminService = new();

        public ICommand TransactionReportCommand { get; }
        public ICommand AccountStatementCommand { get; }
        public ICommand OverspendingReportCommand { get; }

        public AdminReportsPanelVM()
        {
            TransactionReportCommand = new RelayCommand(_ => Export("Transaction Report"));
            AccountStatementCommand = new RelayCommand(_ => Export("Account Statement"));
            OverspendingReportCommand = new RelayCommand(_ => Export("Overspending Report"));
        }

        private void Export(string name) =>
            MessageBox.Show(adminService.ExportReportPlaceholder(name), "Reports",
                MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
