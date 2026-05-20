using System.Text;
using MySql.Data.MySqlClient;
using wpfBudgetSys.Database;
using wpfBudgetSys.Model;

namespace wpfBudgetSys.Repositories
{
    internal class TransactionRepository
    {
        public List<Transaction> GetByUserId(int userId)
        {
            return GetFiltered(userId, null, null, null, 1, int.MaxValue).Items;
        }

        public TransactionQueryResult GetFiltered(
            int userId,
            DateTime? fromDate,
            int? categoryId,
            string? transactionKind,
            int page,
            int pageSize)
        {
            var where = new StringBuilder(" WHERE a.user_id = @UserId ");
            var parameters = new List<MySqlParameter>
            {
                new("@UserId", userId)
            };

            if (fromDate.HasValue)
            {
                where.Append(" AND t.date >= @FromDate ");
                parameters.Add(new MySqlParameter("@FromDate", fromDate.Value));
            }

            if (categoryId.HasValue)
            {
                where.Append(" AND t.category_id = @CategoryId ");
                parameters.Add(new MySqlParameter("@CategoryId", categoryId.Value));
            }

            if (!string.IsNullOrEmpty(transactionKind) && transactionKind != "All")
            {
                switch (transactionKind)
                {
                    case "Deposit":
                        where.Append(" AND t.type = 'Credit' ");
                        break;
                    case "Withdraw":
                        where.Append(" AND t.type = 'Debit' AND t.description = 'Withdraw' ");
                        break;
                    case "Transfer":
                        where.Append(" AND t.related_account_id IS NOT NULL ");
                        break;
                    case "Payment":
                        where.Append(" AND t.type = 'Debit' AND t.category_id IS NOT NULL AND t.related_account_id IS NULL AND t.description <> 'Withdraw' ");
                        break;
                }
            }

            string baseFrom = @"
                FROM transactions t
                INNER JOIN accounts a ON t.account_id = a.account_id
                LEFT JOIN expense_categories ec ON t.category_id = ec.category_id ";

            string countQuery = "SELECT COUNT(*) " + baseFrom + where;

            int offset = (page - 1) * pageSize;
            string dataQuery = @"
                SELECT t.transaction_id, t.account_id, t.category_id, t.related_account_id, t.type, t.amount,
                       t.description, t.reference_number, t.date, ec.category_name "
                + baseFrom + where
                + " ORDER BY t.date DESC LIMIT @PageSize OFFSET @Offset ";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            int totalCount;
            using (MySqlCommand countCmd = new MySqlCommand(countQuery, conn))
            {
                foreach (var p in parameters)
                    countCmd.Parameters.Add(p);
                totalCount = Convert.ToInt32(countCmd.ExecuteScalar());
            }

            var items = new List<Transaction>();
            using (MySqlCommand dataCmd = new MySqlCommand(dataQuery, conn))
            {
                foreach (var p in parameters)
                    dataCmd.Parameters.Add(p);
                dataCmd.Parameters.Add(new MySqlParameter("@PageSize", pageSize));
                dataCmd.Parameters.Add(new MySqlParameter("@Offset", offset));

                using MySqlDataReader reader = dataCmd.ExecuteReader();
                while (reader.Read())
                {
                    items.Add(MapTransaction(reader));
                }
            }

            return new TransactionQueryResult { Items = items, TotalCount = totalCount };
        }

        public decimal GetMonthlyCategorySpend(int userId, int categoryId, DateTime monthStart, DateTime monthEnd)
        {
            const string query = @"
                SELECT COALESCE(SUM(t.amount), 0)
                FROM transactions t
                INNER JOIN accounts a ON t.account_id = a.account_id
                WHERE a.user_id = @UserId
                  AND t.category_id = @CategoryId
                  AND t.type = 'Debit'
                  AND t.date >= @MonthStart AND t.date < @MonthEnd";

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlCommand cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
            cmd.Parameters.AddWithValue("@MonthStart", monthStart);
            cmd.Parameters.AddWithValue("@MonthEnd", monthEnd);

            return Convert.ToDecimal(cmd.ExecuteScalar());
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

        private static Transaction MapTransaction(MySqlDataReader reader) => new()
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
        };
    }
}
