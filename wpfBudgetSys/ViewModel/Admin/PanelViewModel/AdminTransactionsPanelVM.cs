using System.Collections.ObjectModel;
using System.Windows.Input;
using wpfBudgetSys.Model.Admin;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.Admin.PanelViewModel
{
    public class AdminTransactionsPanelVM : ViewModelBase
    {
        private readonly AdminService adminService = new();

        public ObservableCollection<AdminTransactionRow> Transactions { get; } = new();
        public List<string> TypeOptions { get; } = new() { "All", "Credit", "Debit" };

        private DateTime? filterFrom;
        public DateTime? FilterFrom
        {
            get => filterFrom;
            set { filterFrom = value; OnPropertyChanged(); }
        }

        private DateTime? filterTo;
        public DateTime? FilterTo
        {
            get => filterTo;
            set { filterTo = value; OnPropertyChanged(); }
        }

        public string FilterType { get; set; } = "All";

        private string filterAccountNumber = string.Empty;
        public string FilterAccountNumber
        {
            get => filterAccountNumber;
            set { filterAccountNumber = value; OnPropertyChanged(); }
        }

        public ICommand SearchCommand { get; }
        public ICommand FlagCommand { get; }
        public ICommand UnflagCommand { get; }

        public AdminTransactionsPanelVM()
        {
            SearchCommand = new RelayCommand(_ => Load());
            FlagCommand = new RelayCommand(o =>
            {
                if (o is AdminTransactionRow row)
                    adminService.FlagTransaction(row.TransactionId, true);
                Load();
            });
            UnflagCommand = new RelayCommand(o =>
            {
                if (o is AdminTransactionRow row)
                    adminService.FlagTransaction(row.TransactionId, false);
                Load();
            });

            Load();
        }

        private void Load()
        {
            Transactions.Clear();
            foreach (var t in adminService.GetTransactions(FilterFrom, FilterTo, FilterType, null, FilterAccountNumber))
                Transactions.Add(t);
        }
    }
}
