using EquipmentInventory.Forms.Windows;

namespace EquipmentInventory.Classes.Helper
{
    internal class CustomMessageBoxHelper
    {
        public static bool Show(string title, string message, bool YesNo)
        {
            var messageBox = new CustomMessageBox(title, message, !YesNo);
            return messageBox.ShowDialog() ?? false;
        }
    }
}
