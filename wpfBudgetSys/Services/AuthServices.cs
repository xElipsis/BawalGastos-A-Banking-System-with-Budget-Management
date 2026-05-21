using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wpfBudgetSys.Enums;
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

        public void Register(string fullName, string email, string phone, string username, string password)
        {
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
        }

        public bool Login(string username, string password)
        {
            Login login = loginRepository.GetByUsername(username);

            if (login == null)
            {
                MessageBox.Show("Username does not exist.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (login.IsLocked)
            {
                MessageBox.Show("Your account has been locked. Please contact support.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
            SessionManager.Login(user, login);

            return true;
        }
    }
}
