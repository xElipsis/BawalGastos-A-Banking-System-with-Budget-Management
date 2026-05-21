using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Enums;
using wpfBudgetSys.Model;
using wpfBudgetSys.Model.Admin;

namespace wpfBudgetSys.Repositories
{
    internal class UserRepository
    {
        public int InsertUser(User user)
        {
            using(MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO users (role_id, full_name, email, phone, status) VALUES (@Role, @FullName, @Email, @Phone, @Status)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@FullName", user.Fullname);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone);
                    cmd.Parameters.AddWithValue("@Status", user.Status);

                    cmd.ExecuteNonQuery();

                    return (int)cmd.LastInsertedId;
                }
            }
        }

        public User GetById(int userId)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM users WHERE user_id = @UserId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn)) 
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32("user_id"),
                                Role = (AppEnums.UserRole)reader.GetInt32("role_id"),
                                Fullname = reader.GetString("full_name"),
                                Email = reader.GetString("email"),
                                Phone = reader.GetString("phone"),
                                Status = reader.GetString("status")
                            };
                        }
                        return null;
                    }
                }
            }
        }

        public void UpdateProfile(int userId, string fullName, string email, string phone)
        {
            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            const string query = @"
                UPDATE users SET full_name = @FullName, email = @Email, phone = @Phone
                WHERE user_id = @UserId";

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@FullName", fullName);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Phone", phone);
            cmd.ExecuteNonQuery();
        }

        public List<AdminUserRow> GetAllForAdmin()
        {
            var rows = new List<AdminUserRow>();
            const string query = @"
                SELECT u.user_id, u.full_name, u.email, u.phone, u.status,
                       l.login_id, l.username, l.is_locked, l.failed_attempts,
                       a.account_number, a.balance, a.status AS account_status
                FROM users u
                INNER JOIN logins l ON l.user_id = u.user_id
                LEFT JOIN accounts a ON a.user_id = u.user_id
                WHERE u.role_id = @UserRole
                ORDER BY u.full_name";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserRole", (int)AppEnums.UserRole.User);

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rows.Add(new AdminUserRow
                {
                    UserId = reader.GetInt32("user_id"),
                    LoginId = reader.GetInt32("login_id"),
                    FullName = reader.GetString("full_name"),
                    Email = reader.GetString("email"),
                    Phone = reader.GetString("phone"),
                    Status = reader.GetString("status"),
                    Username = reader.GetString("username"),
                    IsLocked = reader.GetBoolean("is_locked"),
                    FailedAttempts = reader.GetInt32("failed_attempts"),
                    AccountNumber = reader.IsDBNull(reader.GetOrdinal("account_number")) ? null : reader.GetString("account_number"),
                    AccountBalance = reader.IsDBNull(reader.GetOrdinal("balance")) ? null : reader.GetDecimal("balance"),
                    AccountStatus = reader.IsDBNull(reader.GetOrdinal("account_status")) ? null : reader.GetString("account_status")
                });
            }

            return rows;
        }

        public void UpdateStatus(int userId, string status)
        {
            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            const string query = "UPDATE users SET status = @Status WHERE user_id = @UserId";
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.ExecuteNonQuery();
        }
    }
}
