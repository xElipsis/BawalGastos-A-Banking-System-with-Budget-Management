namespace wpfBudgetSys.Model
{
    internal class Login
    {
        public string? Username { get; set; }
        public DateTime LastLogin { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LockedUntil { get; set; }
    }
}
