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
        }

        #region Load

        private void InitializeUI()
        {
            titleTab.Text = _title;
        }

        #endregion
    }
}
