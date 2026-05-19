using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using wpfBudgetSys.Model;
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

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ExpenseCategory selected = comboBox?.SelectedItem as ExpenseCategory;
            if (selected == null) return;

            var vm = DataContext as GetStartedPanelVM;
            vm?.AddCategoryCommand.Execute(selected);

            // Reset ComboBox so same item can be re-selected after removal
            comboBox.SelectedItem = null;
        }

        private void NumberOnly_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*\.?[0-9]*$");
            TextBox textBox = sender as TextBox;
            string futureText = textBox.Text.Insert(textBox.CaretIndex, e.Text);
            e.Handled = !regex.IsMatch(futureText);
        }

    }
}
