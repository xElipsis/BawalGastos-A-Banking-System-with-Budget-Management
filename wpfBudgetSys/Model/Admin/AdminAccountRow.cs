namespace wpfBudgetSys.Model.Admin
{
    public class AdminAccountRow
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
