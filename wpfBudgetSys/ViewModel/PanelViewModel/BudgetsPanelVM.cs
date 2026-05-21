using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class BudgetsPanelVM : ViewModelBase
    {
        private readonly BudgetService budgetService = new();

        public ObservableCollection<BudgetLimitRowVM> Budgets { get; }

        private List<ExpenseCategory> availablePresets = new();
        public List<ExpenseCategory> AvailablePresets =>
            availablePresets
                .Where(p => p.CategoryId == 0 ||
                            !Budgets.Any(b => b.CategoryId == p.CategoryId))
                .ToList();

        public ICommand AddBudgetCommand { get; }
        public ICommand SaveAllCommand { get; }
        public ICommand DeleteBudgetCommand { get; }
        public ICommand RefreshCommand { get; }

        private string statusMessage = string.Empty;
        public string StatusMessage
        {
            get => statusMessage;
            set { statusMessage = value; OnPropertyChanged(); }
        }

        public BudgetsPanelVM()
        {
            Budgets = new ObservableCollection<BudgetLimitRowVM>();
            LoadBudgets();
            LoadPresets();

            AddBudgetCommand = new RelayCommand(o =>
            {
                if (o is not ExpenseCategory selected)
                    return;

                if (selected.CategoryId == 0)
                {
                    Budgets.Add(new BudgetLimitRowVM
                    {
                        LimitId = 0,
                        CategoryId = 0,
                        CategoryName = "",
                        IsDefault = false,
                        MonthlyLimit = 0,
                        DailyLimit = 0
                    });
                }
                else
                {
                    Budgets.Add(new BudgetLimitRowVM
                    {
                        LimitId = 0,
                        CategoryId = selected.CategoryId,
                        CategoryName = selected.CategoryName ?? "",
                        IsDefault = true,
                        MonthlyLimit = 0,
                        DailyLimit = 0
                    });
                }

                OnPropertyChanged(nameof(AvailablePresets));
            });

            SaveAllCommand = new RelayCommand(_ =>
            {
                var rows = Budgets.Select(b => (
                    b.LimitId,
                    b.CategoryId,
                    b.CategoryName,
                    b.MonthlyLimit,
                    b.DailyLimit)).ToList();

                string? error = budgetService.SaveAll(rows);
                if (error != null)
                {
                    StatusMessage = error;
                    return;
                }

                StatusMessage = string.Empty;
                Reload();
                MessageBox.Show("All budgets saved.", "Budgets", MessageBoxButton.OK, MessageBoxImage.Information);
            });

            DeleteBudgetCommand = new RelayCommand(o =>
            {
                if (o is not BudgetLimitRowVM row)
                    return;

                if (row.LimitId == 0)
                {
                    Budgets.Remove(row);
                    OnPropertyChanged(nameof(AvailablePresets));
                    return;
                }

                if (MessageBox.Show(
                        $"Remove budget for \"{row.CategoryName}\"?",
                        "Confirm delete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) != MessageBoxResult.Yes)
                    return;

                string? error = budgetService.DeleteBudget(row.LimitId);
                if (error != null)
                {
                    StatusMessage = error;
                    return;
                }

                Reload();
            });

            RefreshCommand = new RelayCommand(_ => Reload());
        }

        private void LoadBudgets()
        {
            Budgets.Clear();
            foreach (var row in budgetService.GetBudgetsForCurrentUser())
            {
                Budgets.Add(new BudgetLimitRowVM
                {
                    LimitId = row.LimitId,
                    CategoryId = row.CategoryId,
                    CategoryName = row.CategoryName,
                    IsDefault = row.IsDefault,
                    MonthlyLimit = row.MonthlyLimit,
                    DailyLimit = row.DailyLimit
                });
            }
        }

        private void LoadPresets() => availablePresets = budgetService.GetAvailablePresetsForCurrentUser();

        private void Reload()
        {
            LoadBudgets();
            LoadPresets();
            OnPropertyChanged(nameof(AvailablePresets));
        }
    }
}
