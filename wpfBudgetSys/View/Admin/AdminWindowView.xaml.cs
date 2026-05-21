using System.Windows;
using wpfBudgetSys.ViewModel.Admin;

namespace wpfBudgetSys.View.Admin
{
    public partial class AdminWindowView : Window
    {
        public AdminWindowView()
        {
            InitializeComponent();
            var vm = new AdminWindowVM();
            vm.CloseAction = Close;
            DataContext = vm;
        }
    }
}
