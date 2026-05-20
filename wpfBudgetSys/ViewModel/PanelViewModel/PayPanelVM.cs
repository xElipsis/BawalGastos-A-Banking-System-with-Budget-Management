using System.Collections.Generic;
using System.Windows.Input;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class PayPanelVM : ViewModelBase
    {
        private readonly AccountServices accountService = new();
        private readonly ExpenseCategoryServices categoryServices = new();

        public List<ExpenseCategory> Categories { get; }

        private string account = string.Empty;
        public string Account
        {
            get => account;
            set { account = value; OnPropertyChanged(); }
        }

        private ExpenseCategory? selectedCategory;
        public ExpenseCategory? SelectedCategory
        {
            get => selectedCategory;
            set { selectedCategory = value; OnPropertyChanged(); }
        }

        private string payee = string.Empty;
        public string Payee
        {
            get => payee;
            set { payee = value; OnPropertyChanged(); }
        }

        private string amount = string.Empty;
        public string Amount
        {
            get => amount;
            set { amount = value; OnPropertyChanged(); }
        }

        public ICommand PayCommand { get; }

        public PayPanelVM()
        {
            var userAccount = accountService.GetAccountForCurrentUser();
            if (userAccount != null)
                Account = userAccount.AccountNumber;

            int userId = SessionManager.CurrentUser?.UserId ?? 0;
            Categories = userId > 0
                ? categoryServices.GetUserBudgetCategories(userId)
                : new List<ExpenseCategory>();

            if (Categories.Count > 0)
                SelectedCategory = Categories[0];

            PayCommand = new RelayCommand(_ =>
            {
                // TODO: wire to pay service
            });
        }
    }
}
