using MySql.Data.MySqlClient;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Repositories;

namespace wpfBudgetSys.Services
{
    internal class NotificationService
    {
        private readonly NotificationRepository notificationRepository = new();

        public List<Notification> GetAllForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return new List<Notification>();

            return notificationRepository.GetByUserId(SessionManager.CurrentUser.UserId);
        }

        public int GetUnreadCountForCurrentUser()
        {
            if (SessionManager.CurrentUser == null)
                return 0;

            return notificationRepository.GetUnreadCount(SessionManager.CurrentUser.UserId);
        }

        public void Send(int userId, string title, string message, string type,
                         MySqlConnection conn, MySqlTransaction? transaction = null)
        {
            Notification notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type
            };

            notificationRepository.Insert(notification, conn, transaction);
            NavBadgeNotifier.Notify();
        }

        public void MarkAsRead(int notificationId)
        {
            notificationRepository.MarkAsRead(notificationId);
            NavBadgeNotifier.Notify();
        }

        public void MarkAllAsRead()
        {
            if (SessionManager.CurrentUser == null)
                return;

            notificationRepository.MarkAllAsRead(SessionManager.CurrentUser.UserId);
            NavBadgeNotifier.Notify();
        }
    }
}
