using System;
using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;

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

            return new Account
            {
                AccountId = reader.GetInt32("account_id"),
                UserId = reader.GetInt32("user_id"),
                AccountNumber = reader.GetString("account_number"),
                AccountType = reader.GetString("account_type"),
                Balance = reader.GetDecimal("balance"),
                Status = reader.GetString("status")
            };
        }

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

            return new Account
            {
                AccountId = reader.GetInt32("account_id"),
                UserId = reader.GetInt32("user_id"),
                AccountNumber = reader.GetString("account_number"),
                AccountType = reader.GetString("account_type"),
                Balance = reader.GetDecimal("balance"),
                Status = reader.GetString("status")
            };
        }
    }
}
