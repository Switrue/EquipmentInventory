using EquipmentInventory.Classes.Services;
using EquipmentInventory.Forms.Windows;

namespace EquipmentInventory.Classes.Helper;

public class CustomMessageBoxHelper
{
    public static bool Show(string title, string message, bool YesNo)
    {
        var messageBox = new CustomMessageBox(title, message, !YesNo);
        return WindowService.ShowDialogWindow(messageBox);
    }

    public static bool Show(string title, string message)
        => Show(title, message, false);

    public static bool Show(string message)
        => Show(string.Empty, message, false);
}
