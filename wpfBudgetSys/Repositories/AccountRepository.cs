using System;
using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;
using wpfBudgetSys.Model.Admin;

namespace wpfBudgetSys.Repositories
{
    internal class AccountRepository
    {
        public bool UserHasAccount(int userId, MySqlConnection conn, MySqlTransaction? transaction = null)
        {
            const string query = "SELECT COUNT(*) FROM accounts WHERE user_id = @UserId";

            using MySqlCommand cmd = transaction == null
                ? new MySqlCommand(query, conn)
                : new MySqlCommand(query, conn, transaction);

            cmd.Parameters.AddWithValue("@UserId", userId);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public bool AccountNumberExists(string accountNumber, MySqlConnection conn, MySqlTransaction? transaction = null)
        {
            const string query = "SELECT COUNT(*) FROM accounts WHERE account_number = @AccountNumber";

            using MySqlCommand cmd = transaction == null
                ? new MySqlCommand(query, conn)
                : new MySqlCommand(query, conn, transaction);

            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        public int Insert(Account account, MySqlConnection conn, MySqlTransaction transaction)
        {
            const string query = @"
                INSERT INTO accounts (user_id, account_number, account_type, balance, status)
                VALUES (@UserId, @AccountNumber, @AccountType, @Balance, @Status)";

            using MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
            cmd.Parameters.AddWithValue("@UserId", account.UserId);
            cmd.Parameters.AddWithValue("@AccountNumber", account.AccountNumber);
            cmd.Parameters.AddWithValue("@AccountType", account.AccountType);
            cmd.Parameters.AddWithValue("@Balance", account.Balance);
            cmd.Parameters.AddWithValue("@Status", account.Status);

            cmd.ExecuteNonQuery();
            return (int)cmd.LastInsertedId;
        }

        public void UpdateBalance(int accountId, decimal amount, MySqlConnection conn, MySqlTransaction transaction)
        {
            const string query = @"UPDATE accounts 
                           SET balance = balance + @Amount 
                           WHERE account_id = @AccountId";

            using MySqlCommand cmd = new MySqlCommand(query, conn, transaction);
            cmd.Parameters.AddWithValue("@Amount", amount);
            cmd.Parameters.AddWithValue("@AccountId", accountId);
            cmd.ExecuteNonQuery();
        }

        public Account? GetById(int accountId)
        {
            const string query = @"
                SELECT account_id, user_id, account_number, account_type, balance, status
                FROM accounts WHERE account_id = @AccountId";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountId", accountId);
            using MySqlDataReader reader = cmd.ExecuteReader();
            return reader.Read() ? MapAccount(reader) : null;
        }

        public Account? GetByUserId(int userId)
        {
            const string query = @"
                SELECT account_id, user_id, account_number, account_type, balance, status
                FROM accounts
                WHERE user_id = @UserId
                LIMIT 1";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;

            return MapAccount(reader);
        }

        public string GetEmailByUserId(int userId)
        {
            const string query = @"
                SELECT email
                FROM users
                WHERE user_id = @UserId
                LIMIT 1";
    
            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
    
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
    
            using MySqlDataReader reader = cmd.ExecuteReader();
            if (!reader.Read())
                return null;
    
            return reader.GetString("email");
        }

        private static Account MapAccount(MySqlDataReader reader) => new()
        {
            AccountId = reader.GetInt32("account_id"),
            UserId = reader.GetInt32("user_id"),
            AccountNumber = reader.GetString("account_number"),
            AccountType = reader.GetString("account_type"),
            Balance = reader.GetDecimal("balance"),
            Status = reader.GetString("status")
        };

        public Account? GetByAccountNumber(string accountNumber)
        {
            const string query = @"SELECT account_id, user_id, account_number, account_type, balance, status
                                   FROM accounts
                                   WHERE account_number = @AccountNumber";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
            using MySqlDataReader reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return MapAccount(reader);
        }

        public List<AdminAccountRow> GetAllForAdmin()
        {
            var rows = new List<AdminAccountRow>();
            const string query = @"
                SELECT a.account_id, a.user_id, a.account_number, a.account_type, a.balance, a.status, a.created_at,
                       u.full_name
                FROM accounts a
                INNER JOIN users u ON a.user_id = u.user_id
                ORDER BY a.created_at DESC";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                rows.Add(new AdminAccountRow
                {
                    AccountId = reader.GetInt32("account_id"),
                    UserId = reader.GetInt32("user_id"),
                    OwnerName = reader.GetString("full_name"),
                    AccountNumber = reader.GetString("account_number"),
                    AccountType = reader.GetString("account_type"),
                    Balance = reader.GetDecimal("balance"),
                    Status = reader.GetString("status"),
                    CreatedAt = reader.GetDateTime("created_at")
                });
            }

            return rows;
        }

        public void UpdateStatus(int accountId, string status)
        {
            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();
            const string query = "UPDATE accounts SET status = @Status WHERE account_id = @AccountId";
            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Status", status);
            cmd.Parameters.AddWithValue("@AccountId", accountId);
            cmd.ExecuteNonQuery();
        }
    }
}
