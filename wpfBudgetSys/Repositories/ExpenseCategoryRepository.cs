using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;

namespace wpfBudgetSys.Repositories
{
    internal class ExpenseCategoryRepository
    {
        public List<ExpenseCategory> GetDefaultCategories()
        {
            List<ExpenseCategory> presetCategories = new List<ExpenseCategory>();

            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM expense_categories WHERE is_default = 1";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            presetCategories.Add(new ExpenseCategory
                            {
                                CategoryId = reader.GetInt32("category_id"),
                                CategoryName = reader.GetString("category_name")
                            });
                        }
                    }
                }
            }
            return presetCategories;
        }
    }
}
