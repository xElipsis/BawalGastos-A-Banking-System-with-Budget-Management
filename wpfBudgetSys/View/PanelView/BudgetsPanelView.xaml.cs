using System.Windows.Controls;
using wpfBudgetSys.Model;
using wpfBudgetSys.ViewModel.PanelViewModel;

namespace wpfBudgetSys.View.PanelView
{
    public partial class BudgetsPanelView : UserControl
    {
        public BudgetsPanelView()
        {
            InitializeComponent();
        }

        private void BudgetCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox comboBox)
                return;

            if (comboBox.SelectedItem is not ExpenseCategory selected)
                return;

            if (DataContext is BudgetsPanelVM vm)
                vm.AddBudgetCommand.Execute(selected);

            comboBox.SelectedItem = null;
        }
    }
}
