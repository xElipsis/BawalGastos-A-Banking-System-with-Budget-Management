using System.Globalization;
using System.Windows.Data;

namespace wpfBudgetSys.Helpers
{
    public class NavMatchConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            string.Equals(value as string, parameter as string, StringComparison.Ordinal);

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isChecked && isChecked && parameter is string nav)
                return nav;

            return Binding.DoNothing;
        }
    }
}
