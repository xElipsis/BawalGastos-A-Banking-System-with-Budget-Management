using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;

namespace wpfBudgetSys.Repositories
{
    internal class TransactionRepository
    {
        public List<Transaction> GetByUserId(int userId)
        {
            var transactions = new List<Transaction>();

            const string query = @"
                SELECT t.transaction_id, t.account_id, t.category_id, t.related_account_id, t.type, t.amount,
                       t.description, t.reference_number, t.date, ec.category_name
                FROM transactions t
                INNER JOIN accounts a ON t.account_id = a.account_id
                LEFT JOIN expense_categories ec ON t.category_id = ec.category_id
                WHERE a.user_id = @UserId
                ORDER BY t.date DESC";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);

            using MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                transactions.Add(new Transaction
                {
                    TransactionId = reader.GetInt32("transaction_id"),
                    AccountId = reader.GetInt32("account_id"),
                    CategoryId = reader.IsDBNull(reader.GetOrdinal("category_id")) ? null : reader.GetInt32("category_id"),
                    RelatedAccountId = reader.IsDBNull(reader.GetOrdinal("related_account_id")) ? null : reader.GetInt32("related_account_id"),
                    Type = reader.GetString("type"),
                    Amount = reader.GetDecimal("amount"),
                    Description = reader.GetString("description"),
                    ReferenceNumber = reader.GetString("reference_number"),
                    Date = reader.GetDateTime("date"),
                    CategoryName = reader.IsDBNull(reader.GetOrdinal("category_name"))
                        ? "—"
                        : reader.GetString("category_name")
                });
            }

            return transactions;
        }

        public void Insert(Transaction transaction, MySqlConnection conn, MySqlTransaction sqlTransaction)
        {
            const string query = @"INSERT INTO transactions 
                           (account_id, category_id, related_account_id, 
                            type, amount, description, reference_number, date)
                           VALUES 
                           (@AccountId, @CategoryId, @RelatedAccountId,
                            @Type, @Amount, @Description, @ReferenceNumber, @Date)";

            using MySqlCommand cmd = new MySqlCommand(query, conn, sqlTransaction);

            cmd.Parameters.AddWithValue("@AccountId", transaction.AccountId);
            cmd.Parameters.AddWithValue("@CategoryId", (object?)transaction.CategoryId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@RelatedAccountId", (object?)transaction.RelatedAccountId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Type", transaction.Type);
            cmd.Parameters.AddWithValue("@Amount", transaction.Amount);
            cmd.Parameters.AddWithValue("@Description", transaction.Description);
            cmd.Parameters.AddWithValue("@ReferenceNumber", transaction.ReferenceNumber);
            cmd.Parameters.AddWithValue("@Date", transaction.Date);
            cmd.ExecuteNonQuery();
        }
    }
}
