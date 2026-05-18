using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class BudgetRowVM : ViewModelBase
    {
        public int CategoryId { get; set; }
        public bool IsDefault { get; set; }

        private string? categoryName;
        public string? CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; OnPropertyChanged(); }
        }

        private decimal dailyLimit;
        public decimal DailyLimit
        {
            get { return dailyLimit; }
            set { dailyLimit = value; OnPropertyChanged(); }
        }

        private decimal monthlyLimit;
        public decimal MonthlyLimit
        {
            get { return monthlyLimit; }
            set { monthlyLimit = value; OnPropertyChanged(); }
        }
    }
}
