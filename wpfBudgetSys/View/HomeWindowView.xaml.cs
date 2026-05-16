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
using wpfBudgetSys.ViewModel;

namespace wpfBudgetSys.View
{
    /// <summary>
    /// Interaction logic for HomeWindowView.xaml
    /// </summary>
    public partial class HomeWindowView : Window
    {
        public HomeWindowView()
        {
            InitializeComponent();
            DataContext = new HomeWindowVM();
        }
    }
}
