using EquipmentInventory.Classes.Data.Export;
using System.Collections.Generic;

namespace EquipmentInventory.Classes.Helpers;

public static class ExportHelper
{
    public static bool Word<T>(List<T> data) => ExportToWord.Export(data);

    public static bool Excel<T>(List<T> data) => ExportToExcel.Export(data);
}
