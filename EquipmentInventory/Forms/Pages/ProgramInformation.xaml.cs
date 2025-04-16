using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Classes.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Pages;

/// <summary>
/// Логика взаимодействия для ProgramInformation.xaml
/// </summary>
public partial class ProgramInformation : UserControl
{
    public ProgramInformation()
    {
        InitializeComponent();
        InitializeParams();
    }

    #region Load
    private void InitializeParams()
    {
        var programInformationService = new ProgramInformationService();
        programInformationService.RegisterControls(titleTextGroupBox, descriptionTxtB);
        DataContext = new ProgramInformationViewModel(programInformationService); ;
    }
    #endregion

    private void Page_MouseDown(object sender, MouseButtonEventArgs e) => Focus();
}
