using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Interfaces;
using EquipmentInventory.Forms.Pages;
using EquipmentInventory.Forms.Pages.Admin;
using EquipmentInventory.Properties;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;


namespace EquipmentInventory.Forms.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IMainWindow
    {
        private UserData _user;
        private UserPanel _userPanel;
        private bool isResizing;
        private double previousWidth;
        private double previousHeight;
        private bool _maximizedWindow;

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

            if (!_maximizedWindow)
            {
                Settings.Default.WindowWidth = Width;
                Settings.Default.WindowHeight = Height;
                Settings.Default.Save();
            }
        }

        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.F1 && Keyboard.Modifiers == ModifierKeys.Control)
            {
                AboutTheProgramm_Click(this, null);
            }
        }

        private void Window_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            // Отключение обработки боковых нажатий мыши
            if (e.ChangedButton == MouseButton.XButton1 || e.ChangedButton == MouseButton.XButton2)
            {
                e.Handled = true; 
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

        public void SaveUserPanelObject(UserPanel userPanel) => _userPanel = userPanel;

        #endregion

        #region Window management

        private async void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                await Task.Delay(200);
                ToggleWindowState();
            }
            else if (!_maximizedWindow)
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

        private void CollapseWindow_Click(object sender, EventArgs e) => WindowState = WindowState.Minimized;

        private void MaximizeWindow_Click(object sender, EventArgs e) => ToggleWindowState();

        private void CloseWindow_Click(object sender, EventArgs e) => Close();

        private void ToggleWindowState()
        {
            var currentScreen = Screen.FromHandle(new WindowInteropHelper(this).Handle);

            Action<Screen> toggleAction = _maximizedWindow ? (Action<Screen>)RestoreWindow : MaximizedWindow;

            toggleAction(currentScreen);
            UpdateUI();
        }

        private void MaximizedWindow(Screen currentScreen)
        {
            previousWidth = Width;
            previousHeight = Height;

            var workingArea = currentScreen.WorkingArea;

            Width = workingArea.Width;
            Height = workingArea.Height;

            Left = workingArea.Left;
            Top = workingArea.Top;

            _maximizedWindow = true;
        }

        private void RestoreWindow(Screen currentScreen)
        {
            Width = previousWidth;
            Height = previousHeight;

            Left = currentScreen.WorkingArea.Left + (currentScreen.WorkingArea.Width - previousWidth) / 2;
            Top = currentScreen.WorkingArea.Top + (currentScreen.WorkingArea.Height - previousHeight) / 2;

            _maximizedWindow = false;
        }

        private void UpdateUI()
        {
            maximizeBtn.Content = _maximizedWindow ? "WindowRestore" : "WindowMaximize";

            resizeMarker.Visibility = _maximizedWindow ? Visibility.Collapsed : Visibility.Visible;

            windowEdging.CornerRadius = _maximizedWindow ? new CornerRadius(0) : new CornerRadius(10);
            footerBorder.CornerRadius = _maximizedWindow ? new CornerRadius(0) : new CornerRadius(0, 0, 10, 10);
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

        private void ResizeHandle_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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

        private void ShowProfileOptions_Click(object sender, RoutedEventArgs e) => actionsPopup.IsOpen = !actionsPopup.IsOpen;

        private void Settings_Click(object sender, RoutedEventArgs e) => UpdatePageWithDefaultSettings(new ProgramSettings());

        private void AboutTheProgramm_Click(object sender, RoutedEventArgs e) => UpdatePageWithDefaultSettings(new ProgramInformation());

        private void Profile_Click(object sender, EventArgs e) => UpdatePageWithDefaultSettings(new UserProfile());

        #endregion

        #region Change frame

        public void ChangeControlPanelFrameContent(Page newContent) => controlPanelFrame.Content = newContent;

        public void ChangeMainFrameContent(Page newContent) => mainFrame.Content = newContent;

        private void UpdatePageWithDefaultSettings(Page newContent)
        {
            _userPanel.SetRadioButtonDefault();
            ChangeMainFrameContent(newContent);
        }

        #endregion
    }
}
