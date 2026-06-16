using System.Windows;
using wpfBudgetSys.View.UserControls;
using wpfBudgetSys.ViewModel;

namespace wpfBudgetSys.View
{
    /// <summary>
    /// Interaction logic for RegisterWindowView.xaml
    /// </summary>
    public partial class RegisterWindowView : Window
    {
        public RegisterWindowView()
        {
            InitializeComponent();
            RegisterWindowVM registerWindowVM = new RegisterWindowVM();
            DataContext = registerWindowVM;
        }
    }
}
