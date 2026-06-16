using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class BudgetLimitRowVM : ViewModelBase
    {
        public int LimitId { get; set; }
        public int CategoryId { get; set; }
        public bool IsDefault { get; set; }

        private string categoryName = string.Empty;
        public string CategoryName
        {
            get => categoryName;
            set { categoryName = value; OnPropertyChanged(); }
        }

        private decimal monthlyLimit;
        public decimal MonthlyLimit
        {
            get => monthlyLimit;
            set { monthlyLimit = value; OnPropertyChanged(); }
        }

        private decimal dailyLimit;
        public decimal DailyLimit
        {
            get => dailyLimit;
            set { dailyLimit = value; OnPropertyChanged(); }
        }
    }
}
