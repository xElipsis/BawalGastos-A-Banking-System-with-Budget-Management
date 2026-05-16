using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;

namespace wpfBudgetSys.Repositories
{
    class UserRepository
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
    }
}
