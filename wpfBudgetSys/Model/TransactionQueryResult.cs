namespace wpfBudgetSys.Model
{
    public class TransactionQueryResult
    {
        public List<Transaction> Items { get; set; } = new();
        public int TotalCount { get; set; }
    }
}
