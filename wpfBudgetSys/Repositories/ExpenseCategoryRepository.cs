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
                                UserId = reader.IsDBNull(reader.GetOrdinal("user_id"))
                                    ? 0
                                    : reader.GetInt32("user_id"),
                                CategoryName = reader.GetString("category_name"),
                                IsDefault = true
                            });
                        }
                    }
                }
            }
            return presetCategories;
        }

        public List<ExpenseCategory> GetCategoriesForUser(int userId)
        {
            var categories = new List<ExpenseCategory>();

            const string query = @"
                SELECT DISTINCT ec.category_id, ec.user_id, ec.category_name, ec.is_default
                FROM expense_categories ec
                INNER JOIN spending_limits sl ON ec.category_id = sl.category_id
                WHERE sl.user_id = @UserId
                ORDER BY ec.category_name";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                categories.Add(new ExpenseCategory
                {
                    CategoryId = reader.GetInt32("category_id"),
                    UserId = reader.IsDBNull(reader.GetOrdinal("user_id"))
                        ? 0
                        : reader.GetInt32("user_id"),
                    CategoryName = reader.GetString("category_name"),
                    IsDefault = reader.GetBoolean("is_default")
                });
            }

            return categories;
        }

        public int InsertCustomCategory(ExpenseCategory category)
        {
            const string query = @"
                INSERT INTO expense_categories (user_id, category_name, is_default)
                VALUES (@UserId, @CategoryName, 0)";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", category.UserId);
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);
            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }

        public int InsertCustomCategory(ExpenseCategory category, MySqlConnection conn, MySqlTransaction transaction)
        {
            const string query = @"
                INSERT INTO expense_categories (user_id, category_name, is_default)
                VALUES (@UserId, @CategoryName, 0)";

            using MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
            cmd.Parameters.AddWithValue("@UserId", category.UserId);
            cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }
    }
}
