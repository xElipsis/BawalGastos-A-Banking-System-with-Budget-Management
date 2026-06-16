using wpfBudgetSys.Helpers;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class AccountSettingsService
    {
        private readonly UserRepository userRepository = new();
        private readonly LoginRepository loginRepository = new();

        public string? UpdateProfile(string fullName, string email, string phone)
        {
            if (SessionManager.CurrentUser == null)
                return "You must be logged in.";

            if (string.IsNullOrWhiteSpace(fullName))
                return "Full name is required.";

            if (string.IsNullOrWhiteSpace(email))
                return "Email is required.";

            int userId = SessionManager.CurrentUser.UserId;
            userRepository.UpdateProfile(userId, fullName.Trim(), email.Trim(), phone?.Trim() ?? "");

            SessionManager.CurrentUser.Fullname = fullName.Trim();
            SessionManager.CurrentUser.Email = email.Trim();
            SessionManager.CurrentUser.Phone = phone?.Trim() ?? "";

            return null;
        }

        public string? UpdateUsername(string newUsername)
        {
            if (SessionManager.CurrentLogin == null)
                return "You must be logged in.";

            if (string.IsNullOrWhiteSpace(newUsername))
                return "Username is required.";

            newUsername = newUsername.Trim();

            if (loginRepository.UsernameExists(newUsername, SessionManager.CurrentLogin.LoginId))
                return "That username is already taken.";

            loginRepository.UpdateUsername(SessionManager.CurrentLogin.LoginId, newUsername);
            SessionManager.CurrentLogin.Username = newUsername;
            return null;
        }

        public string? ChangePassword(string currentPassword, string newPassword, string confirmPassword)
        {
            if (SessionManager.CurrentLogin == null)
                return "You must be logged in.";

            if (string.IsNullOrWhiteSpace(currentPassword) || string.IsNullOrWhiteSpace(newPassword))
                return "Enter your current and new password.";

            if (newPassword != confirmPassword)
                return "New passwords do not match.";

            if (newPassword.Length < 6)
                return "New password must be at least 6 characters.";

            var login = loginRepository.GetByUsername(SessionManager.CurrentLogin.Username);
            if (login == null || !PasswordHelper.Verify(currentPassword, login.PasswordHash))
                return "Current password is incorrect.";

            string hash = PasswordHelper.Hash(newPassword);
            loginRepository.UpdatePasswordHash(SessionManager.CurrentLogin.LoginId, hash);
            return null;
        }
    }
}
