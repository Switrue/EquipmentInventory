using EquipmentInventory.Classes.Models;
using EquipmentInventory.Forms.Pages.Settings_tabs;
using EquipmentInventory.Properties;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Windows
{
    /// <summary>
    /// Логика взаимодействия для ProgramSettings.xaml
    /// </summary>
    public partial class ProgramSettings : Window
    {
        private Dictionary<SettingModel, Page> items;

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
            items = new Dictionary<SettingModel, Page>
            {
                { new SettingModel(Strings.Codes, "Barcode"), new CodesTab(Strings.Codes) },
                { new SettingModel(Strings.Tables, "TableSearch"), new TablesTab() }
            };
            DataContext = items.Keys;
        }

        #endregion

        #region Window management

        private void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void CollapseWindow_Click(object sender, EventArgs e) => WindowState = WindowState.Minimized;

        private void CloseWindow_Click(object sender, EventArgs e) => Close();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

        #endregion

        private void PagesOfSettings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSetting = (SettingModel)((ListView)sender).SelectedItem;

            if (selectedSetting != null && items.TryGetValue(selectedSetting, out var value))
            {
                settingsFrame.Content = value;
            }
        }
    }
}
