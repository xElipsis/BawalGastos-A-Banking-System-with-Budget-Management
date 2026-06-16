namespace wpfBudgetSys.Model.Admin
{
    public class AdminUserRow
    {
        public int UserId { get; set; }
        public int LoginId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
        public int FailedAttempts { get; set; }
        public string? AccountNumber { get; set; }
        public decimal? AccountBalance { get; set; }
        public string? AccountStatus { get; set; }
    }
}
