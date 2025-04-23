using System;
using System.Globalization;
using System.Windows.Data;

namespace EquipmentInventory.Classes.Data.Converters;

public class SelectedQueryToIsEnabledConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string selectedQuery)
        {
            return selectedQuery == null;
        }
        return true;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
