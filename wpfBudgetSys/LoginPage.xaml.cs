using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using wpfBudgetSys.View;

namespace wpfBudgetSys
{
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Register register = new Register();
            //register.Show();
            //this.Close();

            HomeWindowView homeWindow = new HomeWindowView();
            homeWindow.Show();
            Close();
        }

        private void ToRegisterButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindowView registerWindow = new RegisterWindowView();
            registerWindow.Show();
            Close();
        }
    }
}
