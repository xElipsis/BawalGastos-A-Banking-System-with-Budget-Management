namespace wpfBudgetSys.Model
{
    public class Login
    {
        public int LoginId { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public string? PasswordHash { get; set; }
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
