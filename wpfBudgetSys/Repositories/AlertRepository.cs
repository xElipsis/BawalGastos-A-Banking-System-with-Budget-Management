using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;

namespace wpfBudgetSys.Repositories
{
    internal class AlertRepository
    {
        public List<Alert> GetByUserId(int userId)
        {
            var alerts = new List<Alert>();

            const string query = @"
                SELECT a.alert_id, a.user_id, a.category_id, a.alert_type, a.message,
                       a.is_read, a.triggered_at, ec.category_name
                FROM alerts a
                INNER JOIN expense_categories ec ON a.category_id = ec.category_id
                WHERE a.user_id = @UserId
                ORDER BY a.triggered_at DESC";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                alerts.Add(MapAlert(reader));
            }

            return alerts;
        }

        public bool ExistsForMonth(int userId, int categoryId, string alertType, DateTime monthStart, DateTime monthEnd)
        {
            const string query = @"
                SELECT COUNT(*) FROM alerts
                WHERE user_id = @UserId AND category_id = @CategoryId AND alert_type = @AlertType
                  AND triggered_at >= @MonthStart AND triggered_at < @MonthEnd";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            cmd.Parameters.AddWithValue("@AlertType", alertType);
            cmd.Parameters.AddWithValue("@MonthStart", monthStart);
            cmd.Parameters.AddWithValue("@MonthEnd", monthEnd);

            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public void Insert(Alert alert, MySqlConnection conn, MySqlTransaction? transaction = null)
        {
            const string query = @"
                INSERT INTO alerts (user_id, category_id, alert_type, message, is_read, triggered_at)
                VALUES (@UserId, @CategoryId, @AlertType, @Message, @IsRead, @TriggeredAt)";

            using MySqlCommand cmd = transaction == null
                ? new MySqlCommand(query, conn)
                : new MySqlCommand(query, conn, transaction);

            cmd.Parameters.AddWithValue("@UserId", alert.UserId);
            cmd.Parameters.AddWithValue("@CategoryId", alert.CategoryId);
            cmd.Parameters.AddWithValue("@AlertType", alert.AlertType);
            cmd.Parameters.AddWithValue("@Message", alert.Message);
            cmd.Parameters.AddWithValue("@IsRead", alert.IsRead);
            cmd.Parameters.AddWithValue("@TriggeredAt", alert.TriggeredAt);
            cmd.ExecuteNonQuery();
        }

        public void MarkAsRead(int alertId)
        {
            const string query = "UPDATE alerts SET is_read = 1 WHERE alert_id = @AlertId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AlertId", alertId);
            cmd.ExecuteNonQuery();
        }

        private static Alert MapAlert(MySqlDataReader reader) => new()
        {
            AlertId = reader.GetInt32("alert_id"),
            UserId = reader.GetInt32("user_id"),
            CategoryId = reader.GetInt32("category_id"),
            AlertType = reader.GetString("alert_type"),
            Message = reader.GetString("message"),
            IsRead = reader.GetBoolean("is_read"),
            TriggeredAt = reader.GetDateTime("triggered_at"),
            CategoryName = reader.GetString("category_name")
        };
    }
}
