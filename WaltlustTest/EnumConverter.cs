using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace WaltlustTest
{
    public class EnumToVisibilityParamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string ParameterString))
                return false;

            object paramvalue = Enum.Parse(value.GetType(), ParameterString);
            bool returnVal = paramvalue.Equals(value);

            if(returnVal)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string ParameterString))
                return false;

            return Enum.Parse(targetType, ParameterString);
        }
    }

    public class EnumToBooleanParamConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string ParameterString))
                return false;

            object paramvalue = Enum.Parse(value.GetType(), ParameterString);
            bool returnVal = paramvalue.Equals(value);

            return returnVal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(parameter is string ParameterString))
                return false;

            return Enum.Parse(targetType, ParameterString);
        }
    }
}
