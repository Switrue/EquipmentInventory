using System.Collections.Generic;
using System.Globalization;
using System;

namespace EquipmentInventory.Classes.Helpers;

public static class QueryParamsHelper
{
    public static Dictionary<string, string> ToQueryParams(object obj)
    {
        var paramsDict = new Dictionary<string, string>();
        if (obj == null) return paramsDict;

        foreach (var prop in obj.GetType().GetProperties())
        {
            var value = prop.GetValue(obj);
            if (value == null) continue;

            string stringValue;
            if (value is DateTime dateTimeValue)
            {
                stringValue = dateTimeValue.ToString("o"); // ISO 8601 формат
            }
            else if (value is float floatValue)
            {
                stringValue = floatValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is bool boolValue)
            {
                stringValue = boolValue.ToString().ToLower();
            }
            else
            {
                stringValue = value.ToString();
            }

            paramsDict.Add(prop.Name, stringValue);
        }

        return paramsDict;
    }
}
