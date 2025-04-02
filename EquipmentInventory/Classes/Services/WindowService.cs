using System.Windows;

namespace EquipmentInventory.Classes.Services
{
    public class WindowService
    {
        public static void ShowWindow(Window parentWindow, Window dialogWindow) => ConfigureDialogWindow(parentWindow, dialogWindow).Show();

        public static void ShowDialogWindow(Window parentWindow, Window dialogWindow) => ConfigureDialogWindow(parentWindow, dialogWindow).ShowDialog();

        private static Window ConfigureDialogWindow(Window parentWindow, Window dialogWindow)
        {
            dialogWindow.Owner = parentWindow;

            dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            return dialogWindow;
        }
    }
}
