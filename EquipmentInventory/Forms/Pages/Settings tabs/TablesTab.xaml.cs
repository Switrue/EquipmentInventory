using EquipmentInventory.Classes.Models.ViewModels;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Settings_tabs
{
    /// <summary>
    /// Логика взаимодействия для TablesTab.xaml
    /// </summary>
    public partial class TablesTab : Page
    {
        private string _title;

        public TablesTab(string title)
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
            toolTipYearOfObsolescence.ToolTip = Strings.TheIntervalOfObsoleteTechnology;
            HintAssist.SetHint(yearOfObsolescenceCB, Strings.Year);
            applyCodeBtn.Content = Strings.Apply;
        }

        private void InitializeParams()
        {
            DataContext = new ComboBoxesViewModel();
        }

        #endregion

        private void ApplyCode_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(yearOfObsolescenceCB.SelectedItem.ToString());
        }
    }
}
