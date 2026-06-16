namespace wpfBudgetSys.Model.Admin
{
    public class AdminTransactionRow
    {
        public int TransactionId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string OwnerName { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ReferenceNumber { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string? CategoryName { get; set; }
        public bool IsFlagged { get; set; }
    }
}
