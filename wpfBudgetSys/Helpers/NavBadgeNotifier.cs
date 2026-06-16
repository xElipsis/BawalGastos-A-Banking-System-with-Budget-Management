namespace wpfBudgetSys.Helpers
{
    public static class NavBadgeNotifier
    {
        public static event Action? BadgesChanged;

        public static void Notify() => BadgesChanged?.Invoke();
    }
}
