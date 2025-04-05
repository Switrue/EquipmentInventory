using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Settings_tabs;

/// <summary>
/// Логика взаимодействия для CodesTab.xaml
/// </summary>
public partial class CodesTab : UserControl
{
    private string _title;

    public CodesTab(string title)
    {
        InitializeComponent();
        _title = title;
        InitializeUI();
    }

    #region Load

    private void InitializeUI()
    {
        titleTab.Text = _title;
        applyCodeBtn.Content = Strings.Apply;
        HintAssist.SetHint(codeTxtB, Strings.Code);
    }


    #endregion

    private void ApplyCode_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(codeTxtB.Text)) return;

        MessageBox.Show("Код", "Тест");
    }
}
