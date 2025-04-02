using EquipmentInventory.Classes.Models.ViewModels;
using EquipmentInventory.Classes.Services;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProgramInformation.xaml
    /// </summary>
    public partial class ProgramInformation : Page
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

        #region Actions

        private void Page_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

        #endregion
    }
}
