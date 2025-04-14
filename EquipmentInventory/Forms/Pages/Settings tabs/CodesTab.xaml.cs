using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;
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

    #region Methods

    private async void ApplyCode_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(codeTxtB.Text)) return;

        await UserAccountService.ExecuteTask((Button)sender, PerformCodeCheck);
    }

    private async Task PerformCodeCheck()
    {
        var code = new BaseRequest
        {
            Content = codeTxtB.Text
        };

        var result = await SettingsRequest.UseCode(code);

        if (result != null)
        {
            codeTxtB.Text = string.Empty;
            CustomMessageBoxHelper.Show(Strings.Success, result.Message);
        }
    }

    #endregion
}
