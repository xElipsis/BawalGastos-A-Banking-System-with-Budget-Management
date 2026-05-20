using MySql.Data.MySqlClient;
using System.Diagnostics;
using wpfBudgetSys.Database;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class TransactionService
    {
        private readonly TransactionRepository transactionRepository = new();
        private readonly AccountRepository accountRepository = new();

        public List<Transaction> GetTransactionsForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return new List<Transaction>();

            return transactionRepository.GetByUserId(SessionManager.CurrentUser.UserId);
        }

        public void Deposit(int accountId, decimal amount)
        {
            // Validate amount
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            // Wrap both operations in a MySQL transaction
            using MySqlTransaction sqlTransaction = conn.BeginTransaction();

            try
            {
                // Step 1 — update the balance
                accountRepository.UpdateBalance(accountId, amount, conn, sqlTransaction);

                // Step 2 — record the transaction
                Transaction transaction = new Transaction
                {
                    AccountId = accountId,
                    CategoryId = null,
                    RelatedAccountId = null,
                    Type = "Credit",
                    Amount = amount,
                    Description = "Deposit",
                    ReferenceNumber = ReferenceNumberGenerator.Generate(),
                    Date = DateTime.Now
                };

                transactionRepository.Insert(transaction, conn, sqlTransaction);

                // Both succeeded — commit
                sqlTransaction.Commit();
            }
            catch
            {
                // Something failed — roll back both operations
                sqlTransaction.Rollback();
                throw;
            }
        }

        public void Withdraw(int accountId, decimal amount)
        {
            if(amount <= 0)
                throw new ArgumentException("Withdraw amount must be greater than zero.");

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlTransaction sqlTransaction = conn.BeginTransaction();

            try
            {
                accountRepository.UpdateBalance(accountId, -amount, conn, sqlTransaction);

                Transaction transaction = new Transaction
                {
                    AccountId = accountId,
                    CategoryId = null,
                    RelatedAccountId = null,
                    Type = "Debit",
                    Amount = amount,
                    Description = "Withdraw",
                    ReferenceNumber = ReferenceNumberGenerator.Generate(),
                    Date = DateTime.Now
                };

                transactionRepository.Insert(transaction, conn, sqlTransaction);

                sqlTransaction.Commit();
            } 
            catch
            {
                sqlTransaction.Rollback(); 
                throw;
            }
        }

        public void Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            Debug.Write("Hello");

            if (amount <= 0)
                throw new ArgumentException("Transfer amount must be greater than zero.");

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlTransaction sqlTransaction = conn.BeginTransaction();

            try
            {
                accountRepository.UpdateBalance(fromAccountId, -amount, conn, sqlTransaction);
                accountRepository.UpdateBalance(toAccountId, amount, conn, sqlTransaction);

                Transaction transaction = new Transaction
                {
                    AccountId = fromAccountId,
                    CategoryId = null,
                    RelatedAccountId = toAccountId,
                    Type = "Debit",
                    Amount = amount,
                    Description = "Transfer",
                    ReferenceNumber = ReferenceNumberGenerator.Generate(),
                    Date = DateTime.Now
                };

                transactionRepository.Insert(transaction, conn, sqlTransaction);

                sqlTransaction.Commit();
            }
            catch
            {
                sqlTransaction.Rollback();
                throw;
            }
        }
    }
}