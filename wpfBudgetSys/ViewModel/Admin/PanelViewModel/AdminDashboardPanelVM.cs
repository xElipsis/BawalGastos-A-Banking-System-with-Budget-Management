using wpfBudgetSys.Model.Admin;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.Admin.PanelViewModel
{
    public class AdminDashboardPanelVM : ViewModelBase
    {
        private readonly AdminService adminService = new();

        public int TotalUsers { get; private set; }
        public int ActiveAccounts { get; private set; }
        public int LockedAccounts { get; private set; }
        public int TodaysTransactions { get; private set; }
        public string TotalSystemBalance { get; private set; } = "₱0.00";

        public AdminDashboardPanelVM() => Load();

        private void Load()
        {
            AdminDashboardStats stats = adminService.GetDashboardStats();
            TotalUsers = stats.TotalUsers;
            ActiveAccounts = stats.ActiveAccounts;
            LockedAccounts = stats.LockedAccounts;
            TodaysTransactions = stats.TodaysTransactions;
            TotalSystemBalance = $"₱{stats.TotalSystemBalance:N2}";

            OnPropertyChanged(nameof(TotalUsers));
            OnPropertyChanged(nameof(ActiveAccounts));
            OnPropertyChanged(nameof(LockedAccounts));
            OnPropertyChanged(nameof(TodaysTransactions));
            OnPropertyChanged(nameof(TotalSystemBalance));
        }
    }
}
