using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Enums;
using wpfBudgetSys.Model;

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
    }
}
