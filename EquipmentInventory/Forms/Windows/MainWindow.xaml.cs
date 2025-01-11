using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EquipmentInventory.Forms.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isResizing;

        private double previousWidth;

        private double previousHeight;

        public MainWindow()
        {
            InitializeComponent();
            InitializeUI();
        }

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

        #region Load

        private void InitializeUI()
        {
            string username = UserData.Surname + " " + UserData.Username;

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
            userRegistrationRb.Content = Strings.UserRegistration;
            tablesRb.Content = Strings.Tables;
            dictionariesRb.Content = Strings.Dictionaries;

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
                ToggleWindowState(WindowState == WindowState.Normal);
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
                ToggleWindowState(true);
            }
        }

        private void CollapseWindow_Click(object sender, EventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow_Click(object sender, EventArgs e)
        {
            ToggleWindowState(WindowState == WindowState.Normal);
        }

        private void CloseWindow_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            Focus();
        }

        private void ToggleWindowState(bool isMaximized)
        {
            if (isMaximized)
            {
                // Сохраняем текущие размеры перед развертыванием
                previousWidth = Width;
                previousHeight = Height;

                // Устанавливаем состояние окна в развернутое
                WindowState = WindowState.Maximized;
            }
            else
            {
                // Восстанавливаем размеры окна
                Width = previousWidth;
                Height = previousHeight;

                // Устанавливаем состояние окна в нормальное
                WindowState = WindowState.Normal;
            }

            // Обновляем иконку
            maximizeBtn.Content = new PackIcon
            {
                Kind = isMaximized ? PackIconKind.WindowRestore : PackIconKind.WindowMaximize
            };

            resizeMarker.Visibility = isMaximized ? Visibility.Collapsed : Visibility.Visible;
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

        private void ColorChange_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                SolidColorBrush textColor = (SolidColorBrush)Application.Current.Resources["TextColor"];
                SolidColorBrush secondaryColor = (SolidColorBrush)Application.Current.Resources["SecondaryColor"];

                UpdateRadioButtonColors(textColor);

                radioButton.Foreground = secondaryColor;
            }
        }

        private void UpdateRadioButtonColors(SolidColorBrush textColor)
        {
            RadioButton[] elements = { userRegistrationRb, tablesRb, dictionariesRb };

            foreach (var element in elements)
            {
                if (element != null)
                {
                    element.Foreground = textColor;
                }
            }
        }
    }
}
