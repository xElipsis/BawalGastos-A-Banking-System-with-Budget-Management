using System;
using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;

namespace wpfBudgetSys.Repositories
{
    internal class SpendingLimitsRepository
    {
        public int CountByUserId(int userId)
        {
            const string query = "SELECT COUNT(*) FROM spending_limits WHERE user_id = @UserId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public void Insert(SpendingLimits limit, MySqlConnection conn, MySqlTransaction transaction)
        {
            const string query = @"
                INSERT INTO spending_limits (user_id, category_id, monthly_limit, daily_limit)
                VALUES (@UserId, @CategoryId, @MonthlyLimit, @DailyLimit)";

            using MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
            cmd.Parameters.AddWithValue("@UserId", limit.UserId);
            cmd.Parameters.AddWithValue("@CategoryId", limit.CategoryId);
            cmd.Parameters.AddWithValue("@MonthlyLimit", limit.MonthlyLimit);
            cmd.Parameters.AddWithValue("@DailyLimit", limit.DailyLimit);

            cmd.ExecuteNonQuery();
        }
    }
}
