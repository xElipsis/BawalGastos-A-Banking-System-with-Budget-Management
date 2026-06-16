using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace wpfBudgetSys.Helpers
{
    public class IntAtLeastToVisibilityConverter : IValueConverter
    {
        public int Minimum { get; set; } = 1;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int threshold = Minimum;
            if (parameter != null && int.TryParse(parameter.ToString(), out int parsed))
                threshold = parsed;

            int count = value switch
            {
                int i => i,
                long l => (int)l,
                _ => 0
            };

            return count >= threshold ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotSupportedException();
    }
}
