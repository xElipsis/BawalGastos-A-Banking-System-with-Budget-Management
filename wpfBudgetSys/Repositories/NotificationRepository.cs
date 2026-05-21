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
    internal class NotificationRepository
    {
        public List<Notification> GetByUserId(int userId)
        {
            var notifications = new List<Notification>();
            const string query = @"
            SELECT notification_id, user_id, title, message, type, is_read, created_at
            FROM notifications
            WHERE user_id = @UserId
            ORDER BY created_at DESC";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            using MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                notifications.Add(Map(reader));

            return notifications;
        }

        public int GetUnreadCount(int userId)
        {
            const string query = "SELECT COUNT(*) FROM notifications WHERE user_id = @UserId AND (is_read = 0 OR is_read IS NULL)";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Get all unread notifications for a user
        public List<Notification> GetUnreadByUserId(int userId)
        {
            var notifications = new List<Notification>();
            const string query = @"
            SELECT * FROM notifications 
            WHERE user_id = @UserId AND is_read = false
            ORDER BY created_at DESC";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            using MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
                notifications.Add(Map(reader));

            return notifications;
        }

        private static Notification Map(MySqlDataReader reader) => new()
        {
            NotificationId = reader.GetInt32("notification_id"),
            UserId = reader.GetInt32("user_id"),
            Title = reader.GetString("title"),
            Message = reader.GetString("message"),
            Type = reader.GetString("type"),
            IsRead = reader.GetBoolean("is_read"),
            CreatedAt = reader.GetDateTime("created_at")
        };

        // Insert a new notification
        public void Insert(Notification notification,
                           MySqlConnection conn, MySqlTransaction? transaction = null)
        {
            const string query = @"
            INSERT INTO notifications (user_id, title, message, type)
            VALUES (@UserId, @Title, @Message, @Type)";

            using MySqlCommand cmd = transaction == null
                ? new MySqlCommand(query, conn)
                : new MySqlCommand(query, conn, transaction);

            cmd.Parameters.AddWithValue("@UserId", notification.UserId);
            cmd.Parameters.AddWithValue("@Title", notification.Title);
            cmd.Parameters.AddWithValue("@Message", notification.Message);
            cmd.Parameters.AddWithValue("@Type", notification.Type);
            cmd.ExecuteNonQuery();
        }

        // Mark a single notification as read
        public void MarkAsRead(int notificationId)
        {
            const string query = @"
            UPDATE notifications 
            SET is_read = true 
            WHERE notification_id = @NotificationId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@NotificationId", notificationId);
            cmd.ExecuteNonQuery();
        }

        // Mark all notifications as read for a user
        public void InsertStandalone(Notification notification)
        {
            const string query = @"
            INSERT INTO notifications (user_id, title, message, type, is_read, created_at)
            VALUES (@UserId, @Title, @Message, @Type, 0, @CreatedAt)";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", notification.UserId);
            cmd.Parameters.AddWithValue("@Title", notification.Title);
            cmd.Parameters.AddWithValue("@Message", notification.Message);
            cmd.Parameters.AddWithValue("@Type", notification.Type);
            cmd.Parameters.AddWithValue("@CreatedAt", notification.CreatedAt == default ? DateTime.Now : notification.CreatedAt);
            cmd.ExecuteNonQuery();
        }

        public void InsertForAllUsers(string title, string message, string type)
        {
            const string query = @"
            INSERT INTO notifications (user_id, title, message, type, is_read, created_at)
            SELECT u.user_id, @Title, @Message, @Type, 0, @CreatedAt
            FROM users u
            WHERE u.role_id = @UserRole";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Message", message);
            cmd.Parameters.AddWithValue("@Type", type);
            cmd.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
            cmd.Parameters.AddWithValue("@UserRole", 2);
            cmd.ExecuteNonQuery();
        }

        public void MarkAllAsRead(int userId)
        {
            const string query = @"
            UPDATE notifications 
            SET is_read = true 
            WHERE user_id = @UserId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.ExecuteNonQuery();
        }
    }
}
