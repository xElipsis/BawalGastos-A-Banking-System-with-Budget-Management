using System.Collections.ObjectModel;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model.Admin;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.Admin.PanelViewModel
{
    public class AdminNotificationsPanelVM : ViewModelBase
    {
        private readonly AdminService adminService = new();

        public ObservableCollection<AdminUserRow> Users { get; } = new();

        private int selectedUserId;
        public int SelectedUserId
        {
            get => selectedUserId;
            set { selectedUserId = value; OnPropertyChanged(); }
        }

        private string title = string.Empty;
        public string Title
        {
            get => title;
            set { title = value; OnPropertyChanged(); }
        }

        private string message = string.Empty;
        public string Message
        {
            get => message;
            set { message = value; OnPropertyChanged(); }
        }

        public string NotificationType { get; set; } = "system";

        private bool sendToAllUsers;
        public bool SendToAllUsers
        {
            get => sendToAllUsers;
            set { sendToAllUsers = value; OnPropertyChanged(); }
        }

        public ICommand SendCommand { get; }

        public AdminNotificationsPanelVM()
        {
            foreach (var u in adminService.GetUsers())
                Users.Add(u);

            if (Users.Count > 0)
                SelectedUserId = Users[0].UserId;

            SendCommand = new RelayCommand(_ =>
            {
                string? error = SendToAllUsers
                    ? adminService.SendSystemWideNotification(Title, Message, NotificationType)
                    : SelectedUserId <= 0
                        ? "Select a user or enable system-wide."
                        : adminService.SendNotificationToUser(SelectedUserId, Title, Message, NotificationType);

                if (error != null)
                {
                    AppDialog.Show(error, "Notifications", AppDialogIcon.Warning);
                    return;
                }

                Title = string.Empty;
                Message = string.Empty;
                OnPropertyChanged(nameof(Title));
                OnPropertyChanged(nameof(Message));
                AppDialog.Show("Notification sent.", "Notifications", AppDialogIcon.Success);
            });
        }
    }
}
