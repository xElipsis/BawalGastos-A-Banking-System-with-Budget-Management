using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class NotificationItemVM : ViewModelBase
    {
        public NotificationItemVM(Notification notification) => Notification = notification;

        public Notification Notification { get; }

        public int NotificationId => Notification.NotificationId;
        public string Title => Notification.Title;
        public string Message => Notification.Message;
        public string Type => Notification.Type;
        public DateTime CreatedAt => Notification.CreatedAt;

        public bool IsRead
        {
            get => Notification.IsRead;
            set
            {
                if (Notification.IsRead == value) return;
                Notification.IsRead = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ShowMarkReadButton));
            }
        }

        public bool ShowMarkReadButton => !IsRead;
    }
}
