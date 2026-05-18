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
            DataContext = new GetStartedPanelVM();
        }

        private void txtInitialDeposit_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*\.?[0-9]*$");
            TextBox textBox = sender as TextBox;

            // Build what the text would look like after this input
            string futureText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

            // Block the input if it doesn't match the pattern
            e.Handled = !regex.IsMatch(futureText);
        }
    }
}
