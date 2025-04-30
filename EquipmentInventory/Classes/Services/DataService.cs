namespace EquipmentInventory.Classes.Services;

public static class DataService
{
    public static object GetProperty(object item, string propertyName)
    {
        var property = item?.GetType().GetProperty(propertyName);
        if (property != null)
        {
            return property.GetValue(item);
        }

        return null;
    }
}
