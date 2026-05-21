using System.Collections.ObjectModel;
using System.Windows.Input;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class NotificationsPanelVM : ViewModelBase
    {
        private readonly NotificationService notificationService = new();

        public ObservableCollection<NotificationItemVM> Notifications { get; }

        public bool HasNotifications => Notifications.Count > 0;

        public string EmptyMessage =>
            "No notifications yet. You'll see updates here for deposits and other account activity.";

        public ICommand MarkReadCommand { get; }
        public ICommand MarkAllAsReadCommand { get; }

        public NotificationsPanelVM()
        {
            Notifications = new ObservableCollection<NotificationItemVM>();
            Load();

            MarkReadCommand = new RelayCommand(o =>
            {
                if (o is not NotificationItemVM item)
                    return;

                notificationService.MarkAsRead(item.NotificationId);
                item.IsRead = true;
            });

            MarkAllAsReadCommand = new RelayCommand(_ =>
            {
                notificationService.MarkAllAsRead();
                foreach (var item in Notifications)
                    item.IsRead = true;
            });
        }

        private void Load()
        {
            Notifications.Clear();
            foreach (var n in notificationService.GetAllForCurrentUser())
                Notifications.Add(new NotificationItemVM(n));

            OnPropertyChanged(nameof(HasNotifications));
        }
    }
}
