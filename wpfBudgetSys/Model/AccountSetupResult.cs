namespace wpfBudgetSys.Model
{
    public class AccountSetupResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int AccountId { get; set; }
    }
}
