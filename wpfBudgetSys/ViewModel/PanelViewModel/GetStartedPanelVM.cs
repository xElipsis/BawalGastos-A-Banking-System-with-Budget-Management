using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class GetStartedPanelVM : ViewModelBase
    {
        public ObservableCollection<ExpenseCategory> Categories { get; set; }

        public ICommand AddBlankCategoryCommand { get; set; }
        public ICommand AddPresetCategoryCommand { get; set; }
        public ICommand RemoveCategoryCommand { get; set; }

        public GetStartedPanelVM()
        {
            Categories = new ObservableCollection<ExpenseCategory>();

            AddBlankCategoryCommand = new RelayCommand(o => AddBlankCategory());
        }

        private void AddBlankCategory()
        {
            Categories.Add(new ExpenseCategory
            {
                CategoryName = "",
            });
        }
    }
}
