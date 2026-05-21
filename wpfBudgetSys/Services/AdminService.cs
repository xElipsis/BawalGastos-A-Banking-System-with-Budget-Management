using wpfBudgetSys.Enums;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.Model.Admin;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class AdminService
    {
        private readonly UserRepository userRepository = new();
        private readonly LoginRepository loginRepository = new();
        private readonly AccountRepository accountRepository = new();
        private readonly TransactionRepository transactionRepository = new();
        private readonly NotificationRepository notificationRepository = new();

        public AdminDashboardStats GetDashboardStats()
        {
            using var conn = wpfBudgetSys.Database.DBConnection.GetConnection();
            conn.Open();

            int totalUsers = ScalarInt(conn, "SELECT COUNT(*) FROM users WHERE role_id = @Role", ("@Role", (int)AppEnums.UserRole.User));
            int activeAccounts = ScalarInt(conn, "SELECT COUNT(*) FROM accounts WHERE status = 'Active'");
            int lockedAccounts = ScalarInt(conn, @"
                SELECT COUNT(*) FROM logins l
                INNER JOIN users u ON l.user_id = u.user_id
                WHERE u.role_id = @Role AND l.is_locked = 1", ("@Role", (int)AppEnums.UserRole.User));
            int todayTx = transactionRepository.CountToday();
            decimal totalBalance = ScalarDecimal(conn, "SELECT COALESCE(SUM(balance), 0) FROM accounts WHERE status <> 'Closed'");

            return new AdminDashboardStats
            {
                TotalUsers = totalUsers,
                ActiveAccounts = activeAccounts,
                LockedAccounts = lockedAccounts,
                TodaysTransactions = todayTx,
                TotalSystemBalance = totalBalance
            };
        }

        public List<AdminUserRow> GetUsers() => userRepository.GetAllForAdmin();

        public string? SetUserStatus(int userId, string status)
        {
            if (status is not ("Active" or "Suspended"))
                return "Status must be Active or Suspended.";

            userRepository.UpdateStatus(userId, status);
            return null;
        }

        public string? UnlockUser(int loginId)
        {
            loginRepository.UnlockLogin(loginId);
            return null;
        }

        public string? ResetUserPassword(int loginId, string newPassword)
        {
            if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
                return "Password must be at least 6 characters.";

            loginRepository.ResetPassword(loginId, PasswordHelper.Hash(newPassword));
            return null;
        }

        public List<AdminAccountRow> GetAccounts() => accountRepository.GetAllForAdmin();

        public string? SetAccountStatus(int accountId, string status)
        {
            if (status is not ("Active" or "Frozen" or "Closed"))
                return "Invalid account status.";

            accountRepository.UpdateStatus(accountId, status);
            return null;
        }

        public List<AdminTransactionRow> GetTransactions(
            DateTime? fromDate, DateTime? toDate, string? type, int? userId, string? accountNumber) =>
            transactionRepository.GetAllForAdmin(fromDate, toDate, type, userId, accountNumber);

        public void FlagTransaction(int transactionId, bool flagged) =>
            transactionRepository.SetFlagged(transactionId, flagged);

        public string? SendNotificationToUser(int userId, string title, string message, string type)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
                return "Title and message are required.";

            notificationRepository.InsertStandalone(new Notification
            {
                UserId = userId,
                Title = title.Trim(),
                Message = message.Trim(),
                Type = string.IsNullOrWhiteSpace(type) ? "system" : type.Trim(),
                CreatedAt = DateTime.Now
            });

            NavBadgeNotifier.Notify();
            return null;
        }

        public string? SendSystemWideNotification(string title, string message, string type)
        {
            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(message))
                return "Title and message are required.";

            notificationRepository.InsertForAllUsers(title.Trim(), message.Trim(),
                string.IsNullOrWhiteSpace(type) ? "system" : type.Trim());
            NavBadgeNotifier.Notify();
            return null;
        }

        public string ExportReportPlaceholder(string reportName) =>
            $"Crystal Reports export for \"{reportName}\" will be wired here. Reports are generated read-only; transactions are never deleted.";

        private static int ScalarInt(MySql.Data.MySqlClient.MySqlConnection conn, string query, params (string Name, object Value)[] parameters)
        {
            using var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
            foreach (var p in parameters)
                cmd.Parameters.AddWithValue(p.Name, p.Value);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private static decimal ScalarDecimal(MySql.Data.MySqlClient.MySqlConnection conn, string query)
        {
            using var cmd = new MySql.Data.MySqlClient.MySqlCommand(query, conn);
            return Convert.ToDecimal(cmd.ExecuteScalar());
        }
    }
}
