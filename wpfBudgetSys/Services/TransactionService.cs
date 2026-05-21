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
        private readonly AlertService alertService = new();
        private readonly NotificationService notificationService = new();

        public List<Transaction> GetTransactionsForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return new List<Transaction>();

            return transactionRepository.GetByUserId(SessionManager.CurrentUser.UserId);
        }

        public TransactionQueryResult GetFilteredTransactions(
            DateTime? fromDate,
            int? categoryId,
            string? transactionKind,
            int page,
            int pageSize)
        {
            if (SessionManager.CurrentUser == null)
                return new TransactionQueryResult();

            return transactionRepository.GetFiltered(
                SessionManager.CurrentUser.UserId,
                fromDate,
                categoryId,
                transactionKind,
                page,
                pageSize);
        }

        public void Deposit(int accountId, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be greater than zero.");

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlTransaction sqlTransaction = conn.BeginTransaction();

            try
            {
                accountRepository.UpdateBalance(accountId, amount, conn, sqlTransaction);

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

                notificationService.Send(
                    userId: SessionManager.CurrentUser!.UserId,
                    title: "Deposit Successful",
                    message: $"You deposited ₱{amount:N2} to your account.",
                    type: "transaction",
                    conn: conn,
                    transaction: sqlTransaction
                );

                sqlTransaction.Commit();
            }
            catch
            {
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

                notificationService.Send(
                    userId: SessionManager.CurrentUser!.UserId,
                    title: "Withdraw Successful",
                    message: $"You withdrawed ₱{amount:N2} from your account.",
                    type: "transaction",
                    conn: conn,
                    transaction: sqlTransaction
                );

                sqlTransaction.Commit();
            } 
            catch
            {
                sqlTransaction.Rollback(); 
                throw;
            }
        }

        public void Transfer(Account fromAccount, Account toAccount, decimal amount)
        {
            Debug.Write("Hello");

            if (amount <= 0)
                throw new ArgumentException("Transfer amount must be greater than zero.");

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlTransaction sqlTransaction = conn.BeginTransaction();

            try
            {
                accountRepository.UpdateBalance(fromAccount.AccountId, -amount, conn, sqlTransaction);
                accountRepository.UpdateBalance(toAccount.AccountId, amount, conn, sqlTransaction);

                Transaction transaction = new Transaction
                {
                    AccountId = fromAccount.AccountId,
                    CategoryId = null,
                    RelatedAccountId = toAccount.AccountId,
                    Type = "Debit",
                    Amount = amount,
                    Description = "Transfer",
                    ReferenceNumber = ReferenceNumberGenerator.Generate(),
                    Date = DateTime.Now
                };

                transactionRepository.Insert(transaction, conn, sqlTransaction);

                notificationService.Send(
                    userId: SessionManager.CurrentUser!.UserId,
                    title: "Transfer Successful",
                    message: $"You transferred ₱{amount:N2} from your account to account number {toAccount.AccountNumber}.",
                    type: "transaction",
                    conn: conn,
                    transaction: sqlTransaction
                );

                notificationService.Send(
                    userId: SessionManager.CurrentUser!.UserId,
                    title: "Transfer Successful",
                    message: $"Your have received ₱{amount:N2} from account number {fromAccount.AccountNumber}.",
                    type: "transaction",
                    conn: conn,
                    transaction: sqlTransaction
                );

                sqlTransaction.Commit();
            }
            catch
            {
                sqlTransaction.Rollback();
                throw;
            }
        }

        public void Pay(int accountId, ExpenseCategory category, string description, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Payment amount must be greater than zero.");

            using MySqlConnection conn = DBConnection.GetConnection();
            conn.Open();

            using MySqlTransaction sqlTransaction = conn.BeginTransaction();

            try
            {
                accountRepository.UpdateBalance(accountId, -amount, conn, sqlTransaction);

                Transaction transaction = new Transaction
                {
                    AccountId = accountId,
                    CategoryId = category.CategoryId,
                    RelatedAccountId = null,
                    Type = "Debit",
                    Amount = amount,
                    Description = description,
                    ReferenceNumber = ReferenceNumberGenerator.Generate(),
                    Date = DateTime.Now
                };

                transactionRepository.Insert(transaction, conn, sqlTransaction);

                notificationService.Send(
                    userId: SessionManager.CurrentUser!.UserId,
                    title: "Payment Successful",
                    message: $"You payed ₱{amount:N2} to {category.CategoryName}.",
                    type: "transaction",
                    conn: conn,
                    transaction: sqlTransaction
                );

                sqlTransaction.Commit();

                if (SessionManager.CurrentUser != null)
                    alertService.EvaluateBudgetAlertsAfterPayment(SessionManager.CurrentUser.UserId, category.CategoryId);
            }
            catch
            {
                sqlTransaction.Rollback();
                throw;
            }
        }
    }
}