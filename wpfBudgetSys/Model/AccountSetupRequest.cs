namespace wpfBudgetSys.Model
{
    public class AccountSetupRequest
    {
        public int UserId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal InitialDeposit { get; set; }
        public List<BudgetCategorySetupItem> Categories { get; set; } = new();
    }

    public class BudgetCategorySetupItem
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public bool IsDefault { get; set; }
        public decimal MonthlyLimit { get; set; }
        public decimal DailyLimit { get; set; }
    }
}
