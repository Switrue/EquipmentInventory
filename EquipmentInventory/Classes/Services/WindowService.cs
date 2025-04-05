using EquipmentInventory.Classes.Interfaces;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace EquipmentInventory.Classes.Services
{
    public class WindowService : IWindowService
    {
        private readonly Window _window;
        private readonly IWindowState _windowState;

        public bool IsMaximized => _windowState.IsMaximized;

        public WindowService(Window window, IWindowState windowState)
        {
            _window = window;
            _windowState = windowState;
        }

        public static void ShowWindow(Window dialogWindow) =>
            ConfigureDialogWindow(FindParentWindow(), dialogWindow).Show();

        public static bool ShowDialogWindow(Window dialogWindow)
        {
            return ConfigureDialogWindow(FindParentWindow(), dialogWindow).ShowDialog() ?? false;
        }

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

        public void CloseWindow() => _window.Close();

        public void MinimizeWindow() => _window.WindowState = WindowState.Minimized;

        public void ToggleWindowState()
        {
            var currentScreen = Screen.FromHandle(new WindowInteropHelper(_window).Handle);

            Action<Screen> toggleAction = _windowState.IsMaximized ? RestoreWindow : MaximizeWindow;

            toggleAction(currentScreen);
        }

        private void MaximizeWindow(Screen currentScreen)
        {
            _windowState.PreviousWidth = _window.Width;
            _windowState.PreviousHeight = _window.Height;

            var workingArea = currentScreen.WorkingArea;

            _window.Width = workingArea.Width;
            _window.Height = workingArea.Height;
            _window.Left = workingArea.Left;
            _window.Top = workingArea.Top;

            _windowState.IsMaximized = true;
        }

        private void RestoreWindow(Screen currentScreen)
        {
            _window.Width = _windowState.PreviousWidth;
            _window.Height = _windowState.PreviousHeight;
            _window.Left = currentScreen.WorkingArea.Left + (currentScreen.WorkingArea.Width - _windowState.PreviousWidth) / 2;
            _window.Top = currentScreen.WorkingArea.Top + (currentScreen.WorkingArea.Height - _windowState.PreviousHeight) / 2;

            _windowState.IsMaximized = false;
        }
    }
}
