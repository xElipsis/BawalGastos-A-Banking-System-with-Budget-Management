using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class GetStartedPanelVM : ViewModelBase
    {
        private readonly ExpenseCategoryServices expenseCategoryServices = new();
        private readonly AccountSetupServices accountSetupService = new();

        public ObservableCollection<BudgetRowVM> Categories { get; }

        private List<ExpenseCategory> allPresets = new();

        public List<string> AccountTypes { get; } = new() { "Savings" };

        public List<ExpenseCategory> PresetCategories =>
            allPresets
                .Where(p => p.CategoryId == 0 ||
                            !Categories.Any(c => c.CategoryId == p.CategoryId))
                .ToList();

        public ICommand AddCategoryCommand { get; }
        public ICommand RemoveCategoryCommand { get; }
        public ICommand CreateAccountCommand { get; }

        public Action? OnSetupComplete { get; set; }

        public string Fullname => SessionManager.CurrentUser?.Fullname ?? "there";

        private string accountNumber = string.Empty;
        public string AccountNumber
        {
            get => accountNumber;
            set { accountNumber = value; OnPropertyChanged(); }
        }

        private string accountType = "Savings";
        public string AccountType
        {
            get => accountType;
            set { accountType = value; OnPropertyChanged(); }
        }

        private string initialDeposit = string.Empty;
        public string InitialDeposit
        {
            get => initialDeposit;
            set { initialDeposit = value; OnPropertyChanged(); }
        }

        private string statusMessage = string.Empty;
        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                statusMessage = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasStatusMessage));
            }
        }

        public bool HasStatusMessage => !string.IsNullOrWhiteSpace(StatusMessage);

        public event Action<BudgetRowVM>? RowAdded;

        public GetStartedPanelVM()
        {
            Categories = new ObservableCollection<BudgetRowVM>();
            LoadCategories();

            AddCategoryCommand = new RelayCommand(o =>
            {
                if (o is not ExpenseCategory selected)
                    return;

                BudgetRowVM newRow = selected.CategoryId == 0
                    ? new BudgetRowVM
                    {
                        CategoryId = 0,
                        CategoryName = "",
                        MonthlyLimit = 0,
                        DailyLimit = 0,
                        IsDefault = false
                    }
                    : new BudgetRowVM
                    {
                        CategoryId = selected.CategoryId,
                        CategoryName = selected.CategoryName,
                        MonthlyLimit = 0,
                        DailyLimit = 0,
                        IsDefault = true
                    };

                Categories.Add(newRow);
                OnPropertyChanged(nameof(PresetCategories));
                RowAdded?.Invoke(newRow);
            });

            RemoveCategoryCommand = new RelayCommand(o =>
            {
                if (o is not BudgetRowVM row)
                    return;

                Categories.Remove(row);
                OnPropertyChanged(nameof(PresetCategories));
            });

            CreateAccountCommand = new RelayCommand(_ => ExecuteCreateAccount());
        }

        private void ExecuteCreateAccount()
        {
            StatusMessage = string.Empty;

            if (SessionManager.CurrentUser == null)
            {
                StatusMessage = "You must be logged in to create an account.";
                return;
            }

            if (!decimal.TryParse(InitialDeposit, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal deposit)
                && !decimal.TryParse(InitialDeposit, out deposit))
            {
                StatusMessage = "Enter a valid initial deposit amount.";
                return;
            }

            var request = new AccountSetupRequest
            {
                UserId = SessionManager.CurrentUser.UserId,
                AccountNumber = AccountNumber.Trim(),
                AccountType = AccountType,
                InitialDeposit = deposit,
                Categories = Categories.Select(row => new BudgetCategorySetupItem
                {
                    CategoryId = row.CategoryId,
                    CategoryName = row.CategoryName,
                    IsDefault = row.IsDefault,
                    MonthlyLimit = row.MonthlyLimit,
                    DailyLimit = row.DailyLimit
                }).ToList()
            };

            AccountSetupResult result = accountSetupService.CompleteSetup(request);

            if (!result.Success)
            {
                StatusMessage = result.Message;
                return;
            }

            StatusMessage = result.Message;
            MessageBox.Show(result.Message, "Account setup", MessageBoxButton.OK, MessageBoxImage.Information);
            OnSetupComplete?.Invoke();
        }

        private void LoadCategories()
        {
            allPresets = expenseCategoryServices.GetPresetCategories();
            allPresets.Add(new ExpenseCategory
            {
                CategoryId = 0,
                CategoryName = "+ Custom",
                IsDefault = false
            });
        }
    }
}
