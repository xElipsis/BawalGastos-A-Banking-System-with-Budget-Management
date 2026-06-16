using System.Collections.ObjectModel;
using System.Windows.Input;
using wpfBudgetSys.Helpers;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class TransactionPanelVM : ViewModelBase
    {
        private readonly TransactionService transactionService = new();
        private readonly ExpenseCategoryServices categoryServices = new();

        public ObservableCollection<Transaction> Transactions { get; }

        public List<string> DateRangeOptions { get; } = new()
        {
            "Last 7 days",
            "Last 30 days",
            "Last 90 days",
            "All time"
        };

        public List<string> TransactionKindOptions { get; } = new()
        {
            "All",
            "Deposit",
            "Withdraw",
            "Transfer",
            "Payment"
        };

        public List<ExpenseCategory> CategoryFilterOptions { get; }

        public List<int> PageSizeOptions { get; } = new() { 10, 25, 50 };

        private string selectedDateRange = "Last 30 days";
        public string SelectedDateRange
        {
            get => selectedDateRange;
            set { selectedDateRange = value; OnPropertyChanged(); }
        }

        private string selectedTransactionKind = "All";
        public string SelectedTransactionKind
        {
            get => selectedTransactionKind;
            set { selectedTransactionKind = value; OnPropertyChanged(); }
        }

        private ExpenseCategory? selectedCategoryFilter;
        public ExpenseCategory? SelectedCategoryFilter
        {
            get => selectedCategoryFilter;
            set { selectedCategoryFilter = value; OnPropertyChanged(); }
        }

        private int pageSize = 10;
        public int PageSize
        {
            get => pageSize;
            set
            {
                if (pageSize == value) return;
                pageSize = value;
                OnPropertyChanged();
                CurrentPage = 1;
                LoadPage();
            }
        }

        private int currentPage = 1;
        public int CurrentPage
        {
            get => currentPage;
            set { currentPage = value; OnPropertyChanged(); OnPropertyChanged(nameof(PageInfo)); }
        }

        private int totalCount;
        public int TotalCount
        {
            get => totalCount;
            private set { totalCount = value; OnPropertyChanged(); OnPropertyChanged(nameof(PageInfo)); OnPropertyChanged(nameof(TotalPages)); }
        }

        public int TotalPages => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
        public string PageInfo => $"Page {CurrentPage} of {TotalPages} ({TotalCount} total)";
        public bool HasTransactions => Transactions.Count > 0;
        public string EmptyMessage => "No transactions match your filters.";

        public ICommand ApplyFiltersCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand FirstPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand LastPageCommand { get; }

        public TransactionPanelVM()
        {
            int userId = SessionManager.CurrentUser?.UserId ?? 0;
            CategoryFilterOptions = new List<ExpenseCategory>
            {
                new ExpenseCategory { CategoryId = -1, CategoryName = "All categories" }
            };

            if (userId > 0)
                CategoryFilterOptions.AddRange(categoryServices.GetUserBudgetCategories(userId));

            SelectedCategoryFilter = CategoryFilterOptions[0];
            Transactions = new ObservableCollection<Transaction>();

            ApplyFiltersCommand = new RelayCommand(_ => { CurrentPage = 1; LoadPage(); });
            ExportCommand = new RelayCommand(_ => {
                // Get all filtered transactions for the report
                var result = transactionService.GetFilteredTransactions(
                    fromDate: GetFromDate(),
                    categoryId: GetCategoryId(),
                    transactionKind: GetKind(),
                    page: 1,
                    pageSize: int.MaxValue
                );

                // Open save dialog so user picks where to save
                Microsoft.Win32.SaveFileDialog dialog = new()
                {
                    FileName = $"TransactionReport_{DateTime.Now:yyyyMMdd}",
                    DefaultExt = ".docx",
                    Filter = "Word Document (.docx)|*.docx"
                };

                bool? result2 = dialog.ShowDialog();
                if (result2 != true) return;

                // Generate the report
                WordReportHelper.GenerateTransactionReport(result.Items, dialog.FileName);

                // Open the file automatically after saving
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = dialog.FileName,
                    UseShellExecute = true
                });
            });
            FirstPageCommand = new RelayCommand(_ => GoToPage(1), _ => CurrentPage > 1);
            PreviousPageCommand = new RelayCommand(_ => GoToPage(CurrentPage - 1), _ => CurrentPage > 1);
            NextPageCommand = new RelayCommand(_ => GoToPage(CurrentPage + 1), _ => CurrentPage < TotalPages);
            LastPageCommand = new RelayCommand(_ => GoToPage(TotalPages), _ => CurrentPage < TotalPages);

            LoadPage();
        }

        private void GoToPage(int page)
        {
            CurrentPage = page;
            LoadPage();
        }

        private void LoadPage()
        {
            var result = transactionService.GetFilteredTransactions(
                GetFromDate(),    // ← now uses the method
                GetCategoryId(),  // ← now uses the method
                GetKind(),        // ← now uses the method
                CurrentPage,
                PageSize);

            TotalCount = result.TotalCount;
            Transactions.Clear();
            foreach (var t in result.Items)
                Transactions.Add(t);

            OnPropertyChanged(nameof(HasTransactions));
            CommandManager.InvalidateRequerySuggested();
        }

        private DateTime? GetFromDate()
        {
            return SelectedDateRange switch
            {
                "Last 7 days" => DateTime.Now.AddDays(-7),
                "Last 30 days" => DateTime.Now.AddDays(-30),
                "Last 90 days" => DateTime.Now.AddDays(-90),
                _ => null  // "All time" returns null meaning no date filter
            };
        }

        private int? GetCategoryId()
        {
            // -1 is the "All categories" option we set in the constructor
            // If it's -1 or null, return null meaning no category filter
            if (SelectedCategoryFilter == null || SelectedCategoryFilter.CategoryId == -1)
                return null;

            return SelectedCategoryFilter.CategoryId;
        }

        private string? GetKind()
        {
            // "All" means no filter, return null
            if (SelectedTransactionKind == "All")
                return null;

            return SelectedTransactionKind;
        }
    }
}
