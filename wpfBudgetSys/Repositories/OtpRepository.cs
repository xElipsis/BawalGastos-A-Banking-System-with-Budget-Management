using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wpfBudgetSys.Database;

namespace wpfBudgetSys.Repositories
{
    public class OtpRepository
    {
        public void Insert(string email, string otpCode)
        {
            const string query = @"
            INSERT INTO otp_codes (email, otp_code, expires_at)
            VALUES (@Email, @OtpCode, @ExpiresAt)";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@OtpCode", otpCode);
            cmd.Parameters.AddWithValue("@ExpiresAt", DateTime.Now.AddMinutes(5));
            cmd.ExecuteNonQuery();
        }

        // Verify the OTP the user typed
        public bool Verify(string email, string otpCode)
        {
            const string query = @"
            SELECT COUNT(*) FROM otp_codes
            WHERE email      = @Email
            AND   otp_code   = @OtpCode
            AND   expires_at > NOW()
            AND   is_used    = false";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@OtpCode", otpCode);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        // Mark OTP as used after successful verification
        public void MarkAsUsed(string email, string otpCode)
        {
            const string query = @"
            UPDATE otp_codes 
            SET is_used = true
            WHERE email    = @Email
            AND   otp_code = @OtpCode";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@OtpCode", otpCode);
            cmd.ExecuteNonQuery();
        }
    }
}
