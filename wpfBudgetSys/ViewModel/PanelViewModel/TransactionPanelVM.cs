using System.Collections.ObjectModel;
using System.Windows.Input;
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
            ExportCommand = new RelayCommand(_ => { /* User will implement export */ });
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
            DateTime? fromDate = SelectedDateRange switch
            {
                "Last 7 days" => DateTime.Now.AddDays(-7),
                "Last 30 days" => DateTime.Now.AddDays(-30),
                "Last 90 days" => DateTime.Now.AddDays(-90),
                _ => null
            };

            int? categoryId = SelectedCategoryFilter?.CategoryId > 0
                ? SelectedCategoryFilter.CategoryId
                : null;

            string? kind = SelectedTransactionKind == "All" ? null : SelectedTransactionKind;

            var result = transactionService.GetFilteredTransactions(
                fromDate,
                categoryId,
                kind,
                CurrentPage,
                PageSize);

            TotalCount = result.TotalCount;
            Transactions.Clear();
            foreach (var t in result.Items)
                Transactions.Add(t);

            OnPropertyChanged(nameof(HasTransactions));
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
