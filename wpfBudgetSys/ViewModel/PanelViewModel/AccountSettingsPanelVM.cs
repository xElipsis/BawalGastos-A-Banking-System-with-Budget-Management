using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class AccountSettingsPanelVM : ViewModelBase
    {
        private readonly AccountSettingsService accountSettingsService = new();

        public ICommand BackCommand { get; }
        public ICommand SaveProfileCommand { get; }
        public ICommand SaveUsernameCommand { get; }
        public ICommand ChangePasswordCommand { get; }

        private string fullName = string.Empty;
        public string FullName
        {
            get => fullName;
            set { fullName = value; OnPropertyChanged(); }
        }

        private string email = string.Empty;
        public string Email
        {
            get => email;
            set { email = value; OnPropertyChanged(); }
        }

        private string phone = string.Empty;
        public string Phone
        {
            get => phone;
            set { phone = value; OnPropertyChanged(); }
        }

        private string username = string.Empty;
        public string Username
        {
            get => username;
            set { username = value; OnPropertyChanged(); }
        }

        private string currentPassword = string.Empty;
        public string CurrentPassword
        {
            get => currentPassword;
            set { currentPassword = value; OnPropertyChanged(); }
        }

        private string newPassword = string.Empty;
        public string NewPassword
        {
            get => newPassword;
            set { newPassword = value; OnPropertyChanged(); }
        }

        private string confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => confirmPassword;
            set { confirmPassword = value; OnPropertyChanged(); }
        }

        private string statusMessage = string.Empty;
        public string StatusMessage
        {
            get => statusMessage;
            set { statusMessage = value; OnPropertyChanged(); }
        }

        public AccountSettingsPanelVM(ICommand backCommand)
        {
            BackCommand = backCommand;

            if (SessionManager.CurrentUser != null)
            {
                FullName = SessionManager.CurrentUser.Fullname ?? "";
                Email = SessionManager.CurrentUser.Email ?? "";
                Phone = SessionManager.CurrentUser.Phone ?? "";
            }

            if (SessionManager.CurrentLogin != null)
                Username = SessionManager.CurrentLogin.Username ?? "";

            SaveProfileCommand = new RelayCommand(_ =>
            {
                string? error = accountSettingsService.UpdateProfile(FullName, Email, Phone);
                if (error != null)
                {
                    StatusMessage = error;
                    return;
                }

                StatusMessage = string.Empty;
                AppDialog.Show("Profile updated.", "Account settings", AppDialogIcon.Info);
            });

            SaveUsernameCommand = new RelayCommand(_ =>
            {
                string? error = accountSettingsService.UpdateUsername(Username);
                if (error != null)
                {
                    StatusMessage = error;
                    return;
                }

                StatusMessage = string.Empty;
                AppDialog.Show("Username updated.", "Account settings", AppDialogIcon.Info);
            });

            ChangePasswordCommand = new RelayCommand(_ =>
            {
                string? error = accountSettingsService.ChangePassword(CurrentPassword, NewPassword, ConfirmPassword);
                if (error != null)
                {
                    StatusMessage = error;
                    return;
                }

                CurrentPassword = string.Empty;
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
                OnPropertyChanged(nameof(CurrentPassword));
                OnPropertyChanged(nameof(NewPassword));
                OnPropertyChanged(nameof(ConfirmPassword));

                StatusMessage = string.Empty;
                AppDialog.Show("Password changed successfully.", "Account settings", AppDialogIcon.Success);
            });
        }
    }
}
