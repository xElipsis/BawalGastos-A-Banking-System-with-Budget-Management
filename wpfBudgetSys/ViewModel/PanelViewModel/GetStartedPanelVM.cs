using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class GetStartedPanelVM : ViewModelBase
    {
        private readonly ExpenseCategoryServices expenseCategoryServices = new ExpenseCategoryServices();
        public ObservableCollection<BudgetRowVM> Categories { get; set; }

        private List<ExpenseCategory> allPresets = new List<ExpenseCategory>();
        public List<ExpenseCategory> PresetCategories =>
        allPresets
            .Where(p => p.CategoryId == 0 ||  // always show Custom
                   !Categories.Any(c => c.CategoryId == p.CategoryId))
            .ToList();

        public ICommand AddCategoryCommand { get; set; }
        public ICommand RemoveCategoryCommand { get; set; }

        public string Fullname => SessionManager.CurrentUser?.Fullname ?? "there";

        private string initialDeposit = string.Empty;
        public string InitialDeposit
        {
            get => initialDeposit;
            set { initialDeposit = value; OnPropertyChanged(); }
        }

        public event Action<BudgetRowVM>? RowAdded;

        public GetStartedPanelVM()
        {
            Categories = new ObservableCollection<BudgetRowVM>();
            LoadCategories();

            AddCategoryCommand = new RelayCommand(o =>
            {
                ExpenseCategory selected = o as ExpenseCategory;
                if (selected == null) return;

                BudgetRowVM newRow;

                if (selected.CategoryId == 0)
                {
                    newRow = new BudgetRowVM
                    {
                        CategoryId = 0,
                        CategoryName = "",
                        MonthlyLimit = 0,
                        DailyLimit = 0,
                        IsDefault = false
                    };
                } else
                {
                    newRow = new BudgetRowVM
                    {
                        CategoryId = selected.CategoryId,
                        CategoryName = selected.CategoryName,
                        MonthlyLimit = 0,
                        DailyLimit = 0,
                        IsDefault = true
                    };
                }

                Categories.Add(newRow);
                OnPropertyChanged(nameof(PresetCategories));

                RowAdded?.Invoke(newRow);
            });

            RemoveCategoryCommand = new RelayCommand(o =>
            {
                BudgetRowVM row = o as BudgetRowVM;
                if (row == null) return;

                Categories.Remove(row);

                // Preset reappears in dropdown after removal
                OnPropertyChanged(nameof(PresetCategories));
            });
        }

        private void LoadCategories()
        {
            allPresets = expenseCategoryServices.GetPresetCategories();

            // Add the Custom option at the bottom
            allPresets.Add(new ExpenseCategory
            {
                CategoryId = 0,
                CategoryName = "+ Custom",
                IsDefault = false
            });
        }
    }
}
