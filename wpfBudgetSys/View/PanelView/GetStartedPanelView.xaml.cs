using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using wpfBudgetSys.ViewModel.PanelViewModel;

namespace wpfBudgetSys.View.PanelView
{
    /// <summary>
    /// Interaction logic for GetStartedPanelView.xaml
    /// </summary>
    public partial class GetStartedPanelView : UserControl
    {
        public GetStartedPanelView()
        {
            InitializeComponent();
            GetStartedPanelVM getStartedPanelVM = new GetStartedPanelVM();
            DataContext = getStartedPanelVM;
        }
    }
}
