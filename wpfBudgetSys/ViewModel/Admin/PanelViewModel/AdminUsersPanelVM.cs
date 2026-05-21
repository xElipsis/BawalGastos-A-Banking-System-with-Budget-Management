using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Model.Admin;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.Admin.PanelViewModel
{
    public class AdminUsersPanelVM : ViewModelBase
    {
        private readonly AdminService adminService = new();

        public ObservableCollection<AdminUserRow> Users { get; } = new();

        public ICommand RefreshCommand { get; }
        public ICommand ActivateCommand { get; }
        public ICommand SuspendCommand { get; }
        public ICommand UnlockCommand { get; }
        public ICommand ResetPasswordCommand { get; }

        public AdminUsersPanelVM()
        {
            RefreshCommand = new RelayCommand(_ => Load());
            ActivateCommand = new RelayCommand(o => SetStatus(o, "Active"));
            SuspendCommand = new RelayCommand(o => SetStatus(o, "Suspended"));
            UnlockCommand = new RelayCommand(o =>
            {
                if (o is not AdminUserRow row) return;
                adminService.UnlockUser(row.LoginId);
                Load();
                MessageBox.Show($"Unlocked {row.Username}.", "Users", MessageBoxButton.OK, MessageBoxImage.Information);
            });
            ResetPasswordCommand = new RelayCommand(o =>
            {
                if (o is not AdminUserRow row) return;
                string? error = adminService.ResetUserPassword(row.LoginId, "TempPass123!");
                if (error != null)
                {
                    MessageBox.Show(error, "Reset password", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show($"Password reset to TempPass123! for {row.Username}. User must change it after login.",
                    "Users", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            Load();
        }

        private void SetStatus(object? o, string status)
        {
            if (o is not AdminUserRow row) return;
            string? error = adminService.SetUserStatus(row.UserId, status);
            if (error != null)
            {
                MessageBox.Show(error, "Users", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Load();
        }

        private void Load()
        {
            Users.Clear();
            foreach (var u in adminService.GetUsers())
                Users.Add(u);
        }
    }
}
