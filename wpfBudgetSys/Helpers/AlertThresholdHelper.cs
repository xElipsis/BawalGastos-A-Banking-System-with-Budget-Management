using System.Windows.Media;

namespace wpfBudgetSys.Helpers
{
    public static class AlertThresholdHelper
    {
        public static (Brush Border, Brush Background, Brush BadgeForeground) GetColors(string alertType)
        {
            int percent = ParsePercent(alertType);

            return percent switch
            {
                >= 100 => (Brush("#DC2626")!, Brush("#FEE2E2")!, Brush("#991B1B")!),
                >= 90 => (Brush("#EA580C")!, Brush("#FFEDD5")!, Brush("#9A3412")!),
                >= 75 => (Brush("#D97706")!, Brush("#FEF3C7")!, Brush("#92400E")!),
                >= 50 => (Brush("#2563EB")!, Brush("#DBEAFE")!, Brush("#1E40AF")!),
                _ => (Brush("#6B7280")!, Brush("#F3F4F6")!, Brush("#374151")!)
            };
        }

        private static int ParsePercent(string alertType)
        {
            string digits = new string(alertType.Where(char.IsDigit).ToArray());
            return int.TryParse(digits, out int value) ? value : 0;
        }

        private static SolidColorBrush Brush(string hex) =>
            new((Color)ColorConverter.ConvertFromString(hex)!);
    }
}
