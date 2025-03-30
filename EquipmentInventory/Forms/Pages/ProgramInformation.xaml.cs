using EquipmentInventory.Properties;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProgramInformation.xaml
    /// </summary>
    public partial class ProgramInformation : Page
    {
        private Dictionary<string, (string title, string description)> items;

        public ProgramInformation()
        {
            InitializeComponent();
            InitializeUI();
        }

        #region Load

        private void InitializeUI()
        {
            items = new Dictionary<string, (string title, string description)>
            {
                { Strings.Copyrights, (Strings.CopyrightsIllustrations, "- freepic.com\r\n- flaticon.com") },
                { Strings.Vresion, (Strings.Vresion, "5.0.1") },
                { Strings.DateCreation, (Strings.DateAppWasCreated, "27.03.2025") },
                { Strings.ContactDetails, (Strings.ContactDetails, $"{Strings.Email}: kuhtin.kirill2017@mail.ru") },
                { Strings.License, (Strings.License, Strings.AllRightsReserved) },
                { Strings.SubjectArea, (Strings.SubjectArea, Strings.SubjectAreaDescription) },
                { Strings.Developer, (Strings.Developer, Strings.DeveloperDescription) },
                { Strings.SystemRequirements, (Strings.MinimumSystemSequirements, Strings.SystemRequirementsDescription) },
                { Strings.Compatibility, (Strings.Compatibility, "- pgAdmin 4 v8\r\n- postgreSQL 15") }
            };

            var key = items.Keys.ToList();
            DataContext = key;
        }

        #endregion

        #region Actions

        private void Page_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => Keyboard.ClearFocus();

        private void InformationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = informationListV.SelectedItem;

            if (selectedItem == null) return;

            if (items.TryGetValue(selectedItem.ToString(), out var value))
            {
                titleTextGroupBox.Header = value.title;
                descriptionTxtB.Text = value.description;
            }
        }

        #endregion
    }
}
