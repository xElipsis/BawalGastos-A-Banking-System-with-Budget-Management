using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class BudgetRowVM : ViewModelBase
    {
        // ── From ExpenseCategory ──────────────────────────────────
        public int CategoryId { get; set; }
        public bool IsDefault { get; set; }

        private string? categoryName;
        public string? CategoryName
        {
            get => categoryName;
            set { categoryName = value; OnPropertyChanged(); }
        }

        // ── From SpendingLimits ───────────────────────────────────
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

        // Only called for custom categories (IsDefault = false)
        public ExpenseCategory ToExpenseCategory()
        {
            return new ExpenseCategory
            {
                CategoryName = CategoryName,
                UserId = SessionManager.CurrentUser.UserId,
                IsDefault = false
            };
        }

        // Called for every row when saving
        public SpendingLimits ToSpendingLimits(int resolvedCategoryId)
        {
            return new SpendingLimits
            {
                UserId = SessionManager.CurrentUser.UserId,
                CategoryId = resolvedCategoryId,
                MonthlyLimit = MonthlyLimit,
                DailyLimit = DailyLimit
            };
        }
    }
}
