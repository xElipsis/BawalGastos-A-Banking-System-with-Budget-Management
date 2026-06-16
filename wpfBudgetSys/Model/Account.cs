namespace wpfBudgetSys.Model
{
    public class Account
    {
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Status { get; set; } = "Active";

        public override string ToString()
        {
            return $"{AccountType} - {AccountNumber} (Balance: {Balance:C})";
        }
    }
}
