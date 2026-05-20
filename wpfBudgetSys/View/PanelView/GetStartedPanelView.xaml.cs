using System.Windows.Controls;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.ViewModel.PanelViewModel;

namespace wpfBudgetSys.View.PanelView
{
    public partial class GetStartedPanelView : UserControl
    {
        public GetStartedPanelView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (DataContext is not GetStartedPanelVM vm)
                return;

            if (string.IsNullOrWhiteSpace(vm.AccountNumber))
                vm.AccountNumber = AccountNumberGenerator.Generate();
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is not ComboBox comboBox)
                return;

            if (comboBox.SelectedItem is not ExpenseCategory selected)
                return;

            if (DataContext is GetStartedPanelVM vm)
                vm.AddCategoryCommand.Execute(selected);

            comboBox.SelectedItem = null;
        }
    }
}
