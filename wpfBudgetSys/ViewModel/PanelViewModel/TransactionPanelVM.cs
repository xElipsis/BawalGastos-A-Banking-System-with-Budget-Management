using System.Collections.ObjectModel;
using System.Linq;
using wpfBudgetSys.Model;
using wpfBudgetSys.MVVM;
using wpfBudgetSys.Services;

namespace wpfBudgetSys.ViewModel.PanelViewModel
{
    public class TransactionPanelVM : ViewModelBase
    {
        private readonly TransactionService transactionService = new();

        public ObservableCollection<Transaction> Transactions { get; }

        public bool HasTransactions => Transactions.Count > 0;

        public string EmptyMessage => "No transactions yet. Activity from deposits, withdrawals, transfers, and payments will appear here.";

        public TransactionPanelVM()
        {
            Transactions = new ObservableCollection<Transaction>(
                transactionService.GetTransactionsForCurrentUser());

            OnPropertyChanged(nameof(HasTransactions));
            OnPropertyChanged(nameof(EmptyMessage));
        }
    }
}
