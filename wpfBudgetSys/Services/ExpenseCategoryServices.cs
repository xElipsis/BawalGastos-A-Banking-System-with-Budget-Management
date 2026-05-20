using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Model;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class ExpenseCategoryServices
    {
        private readonly ExpenseCategoryRepository expensesCategoryRepo = new ExpenseCategoryRepository();

        public List<ExpenseCategory> GetPresetCategories()
        {
            return expensesCategoryRepo.GetDefaultCategories();
        }

        public List<ExpenseCategory> GetUserBudgetCategories(int userId)
        {
            return expensesCategoryRepo.GetCategoriesForUser(userId);
        }
    }
}
