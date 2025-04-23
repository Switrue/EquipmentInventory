using System.Globalization;
using System.Windows;
using System;
using System.Windows.Data;

namespace EquipmentInventory.Classes.Data.Converters;

public class ValueToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return Visibility.Collapsed;
        }

        if (value is string strValue && string.IsNullOrEmpty(strValue))
        {
            return Visibility.Collapsed;
        }

        if (value is int intValue && intValue == 0)
        {
            return Visibility.Collapsed;
        }

        if (value is bool boolValue && boolValue == false)
        {
            return Visibility.Collapsed;
        }

        return Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
