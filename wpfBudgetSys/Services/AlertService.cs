using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class AlertService
    {
        private static readonly int[] Thresholds = { 50, 75, 90, 100 };

        private readonly AlertRepository alertRepository = new();
        private readonly TransactionRepository transactionRepository = new();
        private readonly SpendingLimitsRepository spendingLimitsRepository = new();

        public List<Alert> GetAlertsForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return new List<Alert>();

            return alertRepository.GetByUserId(SessionManager.CurrentUser.UserId);
        }

        public int GetUnreadCountForCurrentUser() =>
            GetAlertsForCurrentUser().Count(a => !a.IsRead);

        public void MarkAsRead(int alertId) => alertRepository.MarkAsRead(alertId);

        /// <summary>
        /// After a payment, checks monthly spend vs budget and creates alerts at 50/75/90/100% thresholds.
        /// </summary>
        public List<Alert> EvaluateBudgetAlertsAfterPayment(int userId, int categoryId)
        {
            var created = new List<Alert>();

            decimal? monthlyLimit = spendingLimitsRepository.GetMonthlyLimit(userId, categoryId);
            if (monthlyLimit == null || monthlyLimit <= 0)
                return created;

            DateTime monthStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime monthEnd = monthStart.AddMonths(1);

            decimal spent = transactionRepository.GetMonthlyCategorySpend(userId, categoryId, monthStart, monthEnd);
            decimal percent = spent / monthlyLimit.Value * 100m;

            foreach (int threshold in Thresholds)
            {
                if (percent < threshold)
                    continue;

                string alertType = $"{threshold}%";
                if (alertRepository.ExistsForMonth(userId, categoryId, alertType, monthStart, monthEnd))
                    continue;

                var alert = new Alert
                {
                    UserId = userId,
                    CategoryId = categoryId,
                    AlertType = alertType,
                    Message = $"You have reached {threshold}% of your monthly budget for this category (₱{spent:N2} of ₱{monthlyLimit:N2}).",
                    IsRead = false,
                    TriggeredAt = DateTime.Now
                };

                using var dbConn = wpfBudgetSys.Database.DBConnection.GetConnection();
                dbConn.Open();
                alertRepository.Insert(alert, dbConn);
                created.Add(alert);
            }

            return created;
        }
    }
}
