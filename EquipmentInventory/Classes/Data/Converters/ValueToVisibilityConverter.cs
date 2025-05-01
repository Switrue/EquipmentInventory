using System.Globalization;
using System.Windows;
using System;
using System.Windows.Data;

namespace EquipmentInventory.Classes.Data.Converters;

public class ValueToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isVisible = value switch
        {
            null => false,
            string str => !string.IsNullOrEmpty(str),
            int num => num != 0,
            bool flag => flag,
            _ => true
        };

        return isVisible ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
