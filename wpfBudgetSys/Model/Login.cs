namespace wpfBudgetSys.Model
{
    public class Login
    {
        public int LoginId { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime? LastLogin { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? LockedUntil { get; set; }

        public override string ToString()
        {
            return $"LoginId: {LoginId}, UserId: {UserId}, Username: {Username}, LastLogin: {LastLogin}, FailedAttempts: {FailedAttempts}, IsLocked: {IsLocked}, LockedUntil: {LockedUntil}";
        }
    }
}
