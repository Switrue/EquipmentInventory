using System.Linq;
using System.Windows;

namespace EquipmentInventory.Classes.Services
{
    public class WindowService
    {
        public static void ShowWindow(Window dialogWindow) =>
            ConfigureDialogWindow(FindParentWindow(), dialogWindow).Show();

        public static void ShowDialogWindow(Window dialogWindow) =>
            ConfigureDialogWindow(FindParentWindow(), dialogWindow).ShowDialog();

        private static Window ConfigureDialogWindow(Window parentWindow, Window dialogWindow)
        {
            dialogWindow.Owner = parentWindow;
            dialogWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            return dialogWindow;
        }

        private static Window FindParentWindow()
        {
            var activeWindow = Application.Current.Windows.OfType<Window>()
                .FirstOrDefault(w => w.IsActive);

            return activeWindow ?? Application.Current.MainWindow;
        }
    }
}
