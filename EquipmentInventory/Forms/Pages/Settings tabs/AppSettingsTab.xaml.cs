using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Models.ViewModels;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Settings_tabs
{
    /// <summary>
    /// Логика взаимодействия для AppSettingsTab.xaml
    /// </summary>
    public partial class AppSettingsTab : Page
    {
        private string _title;

        public AppSettingsTab(string title)
        {
            InitializeComponent();
            _title = title;
            InitializeUI();
            InitializeParams();
        }

        #region Load

        private void InitializeUI()
        {
            titleTab.Text = _title;
            applyCodeBtn.Content = Strings.Apply;
            HintAssist.SetHint(appLanguageCB, Strings.Language);
        }

        private void InitializeParams()
        {
            DataContext = new ComboBoxesViewModel();
        }

        #endregion

        private void ApplyCode_Click(object sender, RoutedEventArgs e)
        {
            var dialogResult = CustomMessageBoxHelper.Show(Strings.Warning, "?", true);

            if (dialogResult)
            {
                Settings.Default.Save();

                Application.Current.Shutdown();
                Process.Start(Application.ResourceAssembly.Location);
            }
        }
    }
}
