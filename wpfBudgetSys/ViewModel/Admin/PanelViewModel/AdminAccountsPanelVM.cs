using System.Collections.ObjectModel;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model.Admin;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.Admin.PanelViewModel
{
    public class AdminAccountsPanelVM : ViewModelBase
    {
        private readonly AdminService adminService = new();
        private readonly List<AdminAccountRow> allAccounts = new();

        public ObservableCollection<AdminAccountRow> Accounts { get; } = new();

        private AdminAccountRow? selectedAccount;
        public AdminAccountRow? SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasSelection));
                OnPropertyChanged(nameof(CanReactivate));
            }
        }

        public bool HasSelection => SelectedAccount != null;
        public bool CanReactivate => SelectedAccount?.Status == "Closed";

        private string searchText = string.Empty;
        public string SearchText
        {
            get => searchText;
            set { searchText = value; OnPropertyChanged(); ApplyFilter(); }
        }

        public ICommand RefreshCommand { get; }
        public ICommand FreezeCommand { get; }
        public ICommand UnfreezeCommand { get; }
        public ICommand CloseAccountCommand { get; }
        public ICommand ReactivateCommand { get; }

        public AdminAccountsPanelVM()
        {
            RefreshCommand = new RelayCommand(_ => Load());
            FreezeCommand = new RelayCommand(_ => SetStatus("Frozen"), _ => HasSelection);
            UnfreezeCommand = new RelayCommand(_ => SetStatus("Active"), _ => HasSelection);
            CloseAccountCommand = new RelayCommand(_ =>
            {
                if (SelectedAccount == null) return;
                if (!AppDialog.Confirm(
                        $"Close account {SelectedAccount.AccountNumber} for {SelectedAccount.OwnerName}?\n\n" +
                        "The user can still log in but cannot use banking features until an admin reactivates the account.",
                        "Close account"))
                    return;

                SetStatus("Closed");
            }, _ => HasSelection);

            ReactivateCommand = new RelayCommand(_ => SetStatus("Active"), _ => CanReactivate);

            Load();
        }

        private void SetStatus(string status)
        {
            if (SelectedAccount == null) return;
            string? error = adminService.SetAccountStatus(SelectedAccount.AccountId, status);
            if (error != null)
            {
                AppDialog.Show(error, "Account Management", AppDialogIcon.Warning);
                return;
            }

            Load();
            string note = status == "Closed"
                ? "The owner was notified. They do not need to register again — only reactivation restores banking."
                : "Account updated.";
            AppDialog.Show(note, "Account Management", AppDialogIcon.Success);
        }

        private void Load()
        {
            allAccounts.Clear();
            allAccounts.AddRange(adminService.GetAccounts());
            ApplyFilter();
            SelectedAccount = null;
        }

        private void ApplyFilter()
        {
            Accounts.Clear();
            string term = searchText.Trim();
            IEnumerable<AdminAccountRow> filtered = string.IsNullOrEmpty(term)
                ? allAccounts
                : allAccounts.Where(a =>
                    a.OwnerName.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    a.AccountNumber.Contains(term, StringComparison.OrdinalIgnoreCase) ||
                    a.Status.Contains(term, StringComparison.OrdinalIgnoreCase));

            foreach (var a in filtered)
                Accounts.Add(a);
        }
    }
}
