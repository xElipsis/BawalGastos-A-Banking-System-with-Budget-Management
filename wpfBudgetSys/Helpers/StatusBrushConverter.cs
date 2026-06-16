using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace wpfBudgetSys.Helpers
{
    public class StatusBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string status = value?.ToString() ?? string.Empty;
            string kind = parameter?.ToString() ?? "user";

            Color color = kind switch
            {
                "account" => status switch
                {
                    "Active" => Color.FromRgb(0x16, 0xA3, 0x4A),
                    "Frozen" => Color.FromRgb(0xD9, 0x77, 0x06),
                    "Closed" => Color.FromRgb(0xDC, 0x26, 0x26),
                    _ => Color.FromRgb(0x6B, 0x72, 0x80)
                },
                _ => status switch
                {
                    "Active" => Color.FromRgb(0x16, 0xA3, 0x4A),
                    "Suspended" => Color.FromRgb(0xD9, 0x77, 0x06),
                    _ => Color.FromRgb(0x6B, 0x72, 0x80)
                }
            };

            return new SolidColorBrush(color);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
