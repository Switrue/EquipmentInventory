using System.Globalization;
using System.Windows.Data;
using System.Windows;
using System;

namespace EquipmentInventory.Classes.Data.Converters;

public class ReverseValueToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isMatch = value == null
            ? parameter == null
            : parameter != null && value.Equals(parameter);

        return isMatch ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
