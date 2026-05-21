using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Model.Admin;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.Admin.PanelViewModel
{
    public class AdminAccountsPanelVM : ViewModelBase
    {
        private readonly AdminService adminService = new();

        public ObservableCollection<AdminAccountRow> Accounts { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand FreezeCommand { get; }
        public ICommand UnfreezeCommand { get; }
        public ICommand CloseAccountCommand { get; }

        public AdminAccountsPanelVM()
        {
            RefreshCommand = new RelayCommand(_ => Load());
            FreezeCommand = new RelayCommand(o => SetStatus(o, "Frozen"));
            UnfreezeCommand = new RelayCommand(o => SetStatus(o, "Active"));
            CloseAccountCommand = new RelayCommand(o =>
            {
                if (o is not AdminAccountRow row) return;
                if (MessageBox.Show($"Permanently close account {row.AccountNumber}?", "Confirm",
                        MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;

                SetStatus(o, "Closed");
            });

            Load();
        }

        private void SetStatus(object? o, string status)
        {
            if (o is not AdminAccountRow row) return;
            string? error = adminService.SetAccountStatus(row.AccountId, status);
            if (error != null)
            {
                MessageBox.Show(error, "Accounts", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Load();
        }

        private void Load()
        {
            Accounts.Clear();
            foreach (var a in adminService.GetAccounts())
                Accounts.Add(a);
        }
    }
}
