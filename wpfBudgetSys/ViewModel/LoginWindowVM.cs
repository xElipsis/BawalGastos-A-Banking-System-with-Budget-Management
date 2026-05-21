using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using wpfBudgetSys.Enums;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;
using wpfBudgetSys.View;
using wpfBudgetSys.View.Admin;

namespace wpfBudgetSys.ViewModel
{
    class LoginWindowVM : ViewModelBase
    {
        private readonly AuthServices authServices;

        private string inputUsername;

        public string InputUsername
        {
            get { return inputUsername; }
            set 
            { 
                inputUsername = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginUserCommand { get; set; }
        public ICommand ShowRegisterWindowCommand { get; set; }
        public Action CloseAction { get; set; }

        public LoginWindowVM()
        {
            authServices = new AuthServices();

            LoginUserCommand = new RelayCommand(o =>
            {
                PasswordBox passwordBox = o as PasswordBox;
                string password = passwordBox.Password;

                if(string.IsNullOrEmpty(inputUsername) ||
                    string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                bool success = authServices.Login(inputUsername, password);

                if (!success)
                {
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (SessionManager.CurrentUser?.Role == AppEnums.UserRole.Admin)
                {
                    var adminWindow = new AdminWindowView();
                    adminWindow.Show();
                }
                else
                {
                    var homeWindow = new HomeWindowView();
                    homeWindow.Show();
                }

                CloseAction?.Invoke();
            });

            ShowRegisterWindowCommand = new RelayCommand(o =>
            {
                RegisterWindowView registerWindow = new RegisterWindowView();
                registerWindow.Show();
                CloseAction?.Invoke();
            });
        }
    }
}
