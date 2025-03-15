using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Interfaces;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        private UserData _user;

        private bool isResizing;

        private double previousWidth;

        private double previousHeight;

        private bool _maximazedWindow;

        public MainWindow(UserData user)
        {
            InitializeComponent();
            _user = user;
            InitializeUI();
        }

        #region Virtual methods

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            if (WindowState != WindowState.Maximized)
            {
                Settings.Default.WindowWidth = Width;
                Settings.Default.WindowHeight = Height;
                Settings.Default.Save();
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.F1 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AboutTheProgramm_Click(this, null);
            }
        }

        #endregion

        #region Load

        private void InitializeUI()
        {
            string username =  _user.Surname + " " + _user.Username;

            Title = Strings.MainWindowTitle;
            collapseBtn.ToolTip = Strings.Collapse;
            closeBtn.ToolTip = Strings.Close;
            maximizeBtn.ToolTip = Strings.Maximize;
            logOutBtn.Content = Strings.LogOut;
            userNameTexB.Text = string.IsNullOrWhiteSpace(username) ? Strings.DefaultUserName : username;
            profileBtn.Content = Strings.Profile;
            manageAccountBtn.ToolTip = Strings.ManageAccount;
            fileMnIt.Header = $"_{Strings.MainWindowTitle}";
            settingsMnIt.Header = $"_{Strings.Settings}";
            helpMnIt.Header = $"_{Strings.Help}";
            aboutTheProgrammMnIt.Header = $"_{Strings.AboutTheProgramm}";

            Width = Settings.Default.WindowWidth;
            Height = Settings.Default.WindowHeight;
        }

        #endregion

        #region Window management

        private async void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                await Task.Delay(200);
                ToggleWindowState();
            }
            else
            {
                DragMove();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
            {
                ToggleWindowState();
                WindowState = WindowState.Normal;
            }
        }

        private void CollapseWindow_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow_Click(object sender, EventArgs e)
        {
            ToggleWindowState();
        }

        private void CloseWindow_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
        }

        private void ToggleWindowState()
        {
            if (!_maximazedWindow)
            {
                // Сохраняем текущие размеры перед развертыванием
                previousWidth = Width;
                previousHeight = Height;

                // Получаем размеры рабочего стола с учетом панели задач
                var workingArea = SystemParameters.WorkArea;

                // Устанавливаем размеры окна в соответствии с размерами рабочего стола
                Width = workingArea.Width;
                Height = workingArea.Height;

                // Устанавливаем положение окна в верхний левый угол
                Left = workingArea.Left;
                Top = workingArea.Top;

                _maximazedWindow = true;
            }
            else
            {
                // Восстанавливаем размеры окна
                Width = previousWidth;
                Height = previousHeight;

                // Устанавливаем положение окна
                Left = (SystemParameters.PrimaryScreenWidth - previousWidth) / 2;
                Top = (SystemParameters.PrimaryScreenHeight - previousHeight) / 2;

                _maximazedWindow = false;
            }

            // Обновляем иконку
            maximizeBtn.Content = new PackIcon
            {
                Kind = _maximazedWindow ? PackIconKind.WindowRestore : PackIconKind.WindowMaximize
            };

            resizeMarker.Visibility = _maximazedWindow ? Visibility.Collapsed : Visibility.Visible;
        }

        #endregion

        #region Resizing a window

        private void ResizeHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isResizing = true;
            Mouse.Capture(resizeMarker);
        }

        private void ResizeHandle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isResizing = false;
            Mouse.Capture(null);
        }

        private void ResizeHandle_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                var mousePos = e.GetPosition(this);

                // Устанавливаем новые размеры, но не меньше минимальных значений
                double newWidth = mousePos.X + 10;
                double newHeight = mousePos.Y + 10;

                // Проверяем, чтобы новые размеры не были меньше минимальных
                if (newWidth >= MinWidth)
                {
                    Width = newWidth;
                }

                if (newHeight >= MinHeight)
                {
                    Height = newHeight;
                }
            }
        }

        #endregion

        #region Click

        private void Logout_Click(object sender, EventArgs e)
        {
            bool result = CustomMessageBoxHelper.Show(Strings.SignOut, Strings.SignOutDescription, true);

            if (result)
            {
                Settings.Default.Reset();
                Settings.Default.Save();
                new AuthoUser().Show();
                Close();
            }
        }

        private void ShowProfileOptions_Click(object sender, RoutedEventArgs e)
        {
            actionsPopup.IsOpen = !actionsPopup.IsOpen;
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pressed Settings");
        }

        private void AboutTheProgramm_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pressed About the program");
        }

        private void Profile_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Pressed Profile");
        }

        #endregion

        #region Change frame

        public void ChangeMainFrameContent(Page newContent)
        {
            mainFrame.Content = newContent;
        }

        public void ChangeControlPanelFrameContent(Page newContent)
        {
            controlPanelFrame.Content = newContent;
        }

        #endregion
    }
}
