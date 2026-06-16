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

        public List<BudgetLimitRow> GetBudgetRowsByUserId(int userId)
        {
            var rows = new List<BudgetLimitRow>();

            const string query = @"
                SELECT sl.limit_id, sl.category_id, ec.category_name, ec.is_default,
                       sl.monthly_limit, sl.daily_limit
                FROM spending_limits sl
                INNER JOIN expense_categories ec ON sl.category_id = ec.category_id
                WHERE sl.user_id = @UserId
                ORDER BY ec.category_name";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rows.Add(new BudgetLimitRow
                {
                    LimitId = reader.GetInt32("limit_id"),
                    CategoryId = reader.GetInt32("category_id"),
                    CategoryName = reader.GetString("category_name"),
                    IsDefault = reader.GetBoolean("is_default"),
                    MonthlyLimit = reader.GetDecimal("monthly_limit"),
                    DailyLimit = reader.GetDecimal("daily_limit")
                });
            }

            return rows;
        }

        public SpendingLimits? GetByLimitId(int limitId, int userId)
        {
            const string query = @"
                SELECT limit_id, user_id, category_id, monthly_limit, daily_limit
                FROM spending_limits
                WHERE limit_id = @LimitId AND user_id = @UserId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@LimitId", limitId);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            return new SpendingLimits
            {
                LimitId = reader.GetInt32("limit_id"),
                UserId = reader.GetInt32("user_id"),
                CategoryId = reader.GetInt32("category_id"),
                MonthlyLimit = reader.GetDecimal("monthly_limit"),
                DailyLimit = reader.GetDecimal("daily_limit")
            };
        }

        public bool ExistsForUserCategory(int userId, int categoryId)
        {
            const string query = "SELECT COUNT(*) FROM spending_limits WHERE user_id = @UserId AND category_id = @CategoryId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
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

        public void InsertStandalone(SpendingLimits limit)
        {
            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlTransaction tx = conn.BeginTransaction();
            try
            {
                Insert(limit, conn, tx);
                tx.Commit();
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public void Update(SpendingLimits limit)
        {
            const string query = @"
                UPDATE spending_limits
                SET monthly_limit = @MonthlyLimit, daily_limit = @DailyLimit
                WHERE limit_id = @LimitId AND user_id = @UserId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@MonthlyLimit", limit.MonthlyLimit);
            cmd.Parameters.AddWithValue("@DailyLimit", limit.DailyLimit);
            cmd.Parameters.AddWithValue("@LimitId", limit.LimitId);
            cmd.Parameters.AddWithValue("@UserId", limit.UserId);
            cmd.ExecuteNonQuery();
        }

        public void Delete(int limitId, int userId)
        {
            const string query = "DELETE FROM spending_limits WHERE limit_id = @LimitId AND user_id = @UserId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@LimitId", limitId);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.ExecuteNonQuery();
        }

        public decimal? GetMonthlyLimit(int userId, int categoryId)
        {
            const string query = @"
                SELECT monthly_limit FROM spending_limits
                WHERE user_id = @UserId AND category_id = @CategoryId
                LIMIT 1";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);

            object? result = cmd.ExecuteScalar();
            return result == null ? null : Convert.ToDecimal(result);
        }
    }
}
