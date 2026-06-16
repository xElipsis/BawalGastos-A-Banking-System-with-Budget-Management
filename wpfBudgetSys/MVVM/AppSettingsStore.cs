namespace wpfBudgetSys.MVVM
{
    public static class AppSettingsStore
    {
        public static bool ShowBalanceOnDashboard { get; set; } = true;
        public static bool EnableBudgetAlertEmails { get; set; }
        public static bool CompactTransactionList { get; set; }
    }
}
