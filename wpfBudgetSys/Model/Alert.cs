namespace wpfBudgetSys.Model
{
    public class Alert
    {
        public int AlertId { get; set; }
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string AlertType { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime TriggeredAt { get; set; }
        public string? CategoryName { get; set; }
    }
}
