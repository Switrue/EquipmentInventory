using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Settings_tabs;

/// <summary>
/// Логика взаимодействия для TablesTab.xaml
/// </summary>
public partial class TablesTab : UserControl
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
    }

    private void InitializeParams()
    {
        DataContext = new SettingsComboBoxesViewModel();
    }
    #endregion
}
