using System.Collections.ObjectModel;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model.Admin;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.Admin.PanelViewModel
{
    public class AdminUsersPanelVM : ViewModelBase
    {
        private readonly AdminService adminService = new();
        private readonly List<AdminUserRow> allUsers = new();

        public ObservableCollection<AdminUserRow> Users { get; } = new();

        private AdminUserRow? selectedUser;
        public AdminUserRow? SelectedUser
        {
            get => selectedUser;
            set { selectedUser = value; OnPropertyChanged(); OnPropertyChanged(nameof(HasSelection)); }
        }

        public bool HasSelection => SelectedUser != null;

        private string searchText = string.Empty;
        public string SearchText
        {
            get => searchText;
            set { searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand ActivateCommand { get; }
        public ICommand SuspendCommand { get; }
        public ICommand UnlockCommand { get; }
        public ICommand ResetPasswordCommand { get; }

        public AdminUsersPanelVM()
        {
            RefreshCommand = new RelayCommand(_ => Load());
            ActivateCommand = new RelayCommand(_ => SetStatus("Active"), _ => HasSelection);
            SuspendCommand = new RelayCommand(_ => SetStatus("Suspended"), _ => HasSelection);
            UnlockCommand = new RelayCommand(_ =>
            {
                if (SelectedUser == null) return;
                adminService.UnlockUser(SelectedUser.LoginId);
                Load();
                AppDialog.Show($"Unlocked {SelectedUser.Username}.", "User Management", AppDialogIcon.Success);
            }, _ => HasSelection);
            ResetPasswordCommand = new RelayCommand(_ =>
            {
                if (SelectedUser == null) return;
                if (!AppDialog.Confirm($"Reset password for {SelectedUser.Username} to TempPass123!?", "Reset password"))
                    return;

                string? error = adminService.ResetUserPassword(SelectedUser.LoginId, "TempPass123!");
                if (error != null)
                {
                    AppDialog.Show(error, "Reset password", AppDialogIcon.Warning);
                    return;
                }

                AppDialog.Show("Temporary password set to TempPass123!. The user should change it after login.",
                    "User Management", AppDialogIcon.Success);
            }, _ => HasSelection);

            Load();
        }

        private void SetStatus(string status)
        {
            if (SelectedUser == null) return;
            string? error = adminService.SetUserStatus(SelectedUser.UserId, status);
            if (error != null)
            {
                AppDialog.Show(error, "User Management", AppDialogIcon.Warning);
                return;
            }

            Load();
            AppDialog.Show($"User status set to {status}.", "User Management", AppDialogIcon.Success);
        }

        private void Load()
        {
            allUsers.Clear();
            allUsers.AddRange(adminService.GetUsers());
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            Users.Clear();
            string term = searchText.Trim();
            IEnumerable<AdminUserRow> filtered = string.IsNullOrEmpty(term)
                ? allUsers
                : allUsers.Where(u =>
                    u.FullName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    u.Username.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    u.Email.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    (u.AccountNumber?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false));

            foreach (var u in filtered)
                Users.Add(u);
        }
    }
}
