using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Enums;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class AuthServices
    {
        private readonly UserRepository userRepository = new UserRepository();
        private readonly LoginRepository loginRepository = new LoginRepository();
        private readonly OtpService otpService = new OtpService();

        public bool Register(string fullName, string email, string phone, string username, string password, string otpCode)
        {
            bool isValid = otpService.VerifyOtp(email, otpCode);

            if (!isValid)
                return false;

            User newUser = new User
            {
                Role = AppEnums.UserRole.User,
                Fullname = fullName,
                Email = email,
                Phone = phone,
                Status = "Active"
            };
            int newUserId = userRepository.InsertUser(newUser);

            string hashedPassword = PasswordHelper.Hash(password);

            Login newLogin = new Login
            {
                UserId = newUserId,
                Username = username,
            };

            loginRepository.InsertLogin(newLogin, hashedPassword);
            return true;
        }

        public bool Login(string username, string password)
        {
            Login login = loginRepository.GetByUsername(username);

            if (login == null)
            {
                AppDialog.Show("Username does not exist.", "Sign in", AppDialogIcon.Error);
                return false;
            }

            if (login.IsLocked)
            {
                AppDialog.Show("Your account has been locked. Please contact support.", "Sign in", AppDialogIcon.Error);
                return false;
            }

            bool isValid = PasswordHelper.Verify(password, login.PasswordHash);

            if (!isValid)
            {
                loginRepository.IncrementFailedAttempts(login.LoginId);
                return false;
            }

            login.PasswordHash = null;
            User user = userRepository.GetById(login.UserId);
            if (user.Status == "Suspended")
            {
                AppDialog.Show("Your account has been suspended. Please contact support.", "Account suspended", AppDialogIcon.Warning);
                return false;
            }

            loginRepository.UpdateLastLogin(login.LoginId);
            SessionManager.Login(user, login);

            return true;
        }

        public void SendRegistrationOtp(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required.");

            otpService.SendOtp(email.Trim());
        }
    }
}
