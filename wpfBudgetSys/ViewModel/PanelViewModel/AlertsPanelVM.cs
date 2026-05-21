using System.Collections.ObjectModel;
using System.Windows.Input;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class AlertsPanelVM : ViewModelBase
    {
        private readonly AlertService alertService = new();

        public ObservableCollection<Alert> Alerts { get; }

        public bool HasAlerts => Alerts.Count > 0;

        public string EmptyMessage => "No budget alerts yet. Alerts appear when payments reach 50%, 75%, 90%, or 100% of a category's monthly limit.";

        public ICommand MarkReadCommand { get; }
        public ICommand RefreshCommand { get; }

        public AlertsPanelVM()
        {
            Alerts = new ObservableCollection<Alert>(alertService.GetAlertsForCurrentUser());

            MarkReadCommand = new RelayCommand(o =>
            {
                if (o is not Alert alert)
                    return;

                alertService.MarkAsRead(alert.AlertId);
                alert.IsRead = true;
                OnPropertyChanged(nameof(Alerts));
            });

            RefreshCommand = new RelayCommand(_ => Refresh());
        }

        private void Refresh()
        {
            Alerts.Clear();
            foreach (var alert in alertService.GetAlertsForCurrentUser())
                Alerts.Add(alert);

            OnPropertyChanged(nameof(HasAlerts));
        }
    }
}
