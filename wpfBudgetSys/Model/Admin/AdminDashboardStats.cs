namespace wpfBudgetSys.Model.Admin
{
    public class AdminDashboardStats
    {
        public int TotalUsers { get; set; }
        public int ActiveAccounts { get; set; }
        public int LockedAccounts { get; set; }
        public int TodaysTransactions { get; set; }
        public decimal TotalSystemBalance { get; set; }
    }
}
