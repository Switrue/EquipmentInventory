using EquipmentInventory.Classes.Models.ViewModels;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using System;
using System.Windows;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProgramSettings.xaml
    /// </summary>
    public partial class ProgramSettings : Window
    {
        public ProgramSettings()
        {
            InitializeComponent();
            InitializeUI();
            InitializeParams();
        }

        #region Load

        private void InitializeUI()
        {
            windowTitle.Text = Strings.Settings;
            Title = windowTitle.Text;
            closeBtn.ToolTip = Strings.Close;
        }

        private void InitializeParams()
        {
            var navigationService = new NavigationService();
            navigationService.RegisterFrame(settingsFrame);
            DataContext = new SettingsViewModel(navigationService); ;
        }

        #endregion

        #region Window management

        private void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void CollapseWindow_Click(object sender, EventArgs e) => WindowState = WindowState.Minimized;

        private void CloseWindow_Click(object sender, EventArgs e) => Close();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

        #endregion
    }
}
