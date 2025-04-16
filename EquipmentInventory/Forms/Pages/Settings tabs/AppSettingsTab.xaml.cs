using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Settings_tabs;

/// <summary>
/// Логика взаимодействия для AppSettingsTab.xaml
/// </summary>
public partial class AppSettingsTab : UserControl
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
        DataContext = new SettingsComboBoxesViewModel();
    }
    #endregion
}
