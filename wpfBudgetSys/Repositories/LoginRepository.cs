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
    internal class LoginRepository
    {
        public void InsertLogin(Login login, string password_hash)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "INSERT INTO logins (user_id, username, password_hash) VALUES (@User, @Username, @PasswordHash)";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@User", login.UserId);
                    cmd.Parameters.AddWithValue("@Username", login.Username);
                    cmd.Parameters.AddWithValue("@PasswordHash", password_hash);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Login GetByUsername(string username)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = "SELECT * FROM logins WHERE username = @Username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Login
                            {
                                LoginId = reader.GetInt32("login_id"),
                                UserId = reader.GetInt32("user_id"),
                                Username = reader.GetString("username"),
                                PasswordHash = reader.GetString("password_hash"),
                                LastLogin = (reader.IsDBNull(reader.GetOrdinal("last_login")) ? null : reader.GetDateTime("last_login")),
                                FailedAttempts = reader.GetInt32("failed_attempts"),
                                IsLocked = reader.GetBoolean("is_locked"),
                                LockedUntil = (reader.IsDBNull(reader.GetOrdinal("locked_until")) ? null : reader.GetDateTime("locked_until"))
                            };
                        }

                        return null;
                    }
                }
            }
        }

        public void IncrementFailedAttempts(int loginId)
        {
            using (MySqlConnection conn = DBConnection.GetConnection())
            {
                conn.Open();
                string query = @"UPDATE logins 
                                SET failed_attempts = failed_attempts + 1, 
                                is_locked = CASE WHEN failed_attempts + 1 >= 4 
                                THEN true 
                                ELSE false 
                                END 
                                WHERE login_id = @LoginId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LoginId", loginId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
