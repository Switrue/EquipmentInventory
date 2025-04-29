using System;
using System.Globalization;
using System.Windows.Data;

namespace EquipmentInventory.Classes.Data.Converters;

public class SelectedQueryToIsDisabledConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is object selectedQuery)
        {
            return selectedQuery != null;
        }
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
