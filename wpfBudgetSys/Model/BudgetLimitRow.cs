namespace wpfBudgetSys.Model
{
    public class BudgetLimitRow
    {
        public int LimitId { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public decimal MonthlyLimit { get; set; }
        public decimal DailyLimit { get; set; }
    }
}
