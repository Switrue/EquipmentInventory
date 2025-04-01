using EquipmentInventory.Classes.Interfaces;
using EquipmentInventory.Properties;
using System.Collections.ObjectModel;

namespace EquipmentInventory.Classes.Models.ViewModels
{
    public class ProgramInformationViewModel
    {
        private IProgramInformationService _programInformationService;
        private ProgramInformationItem _selectedItem;

        public ObservableCollection<ProgramInformationItem> Items { get; }
        public ProgramInformationItem SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;

                if (_selectedItem != null)
                {
                    _programInformationService.SetTitle(_selectedItem.Title);
                    _programInformationService.SetDescription(_selectedItem.Description);
                }
            }
        }

        public ProgramInformationViewModel(IProgramInformationService programInformationService)
        {
            _programInformationService = programInformationService;

            Items = new ObservableCollection<ProgramInformationItem>()
            {
                new ProgramInformationItem(
                    tabHeader: Strings.Copyrights, 
                    title: Strings.CopyrightsIllustrations, 
                    description: "- freepic.com\r\n- flaticon.com"),

                new ProgramInformationItem(
                    tabHeader: Strings.Vresion, 
                    title: Strings.Vresion, 
                    description: "5.0.1"),

                new ProgramInformationItem(
                    tabHeader: Strings.DateCreation, 
                    title: Strings.DateAppWasCreated, 
                    description: "27.03.2025"),

                new ProgramInformationItem(
                    tabHeader: Strings.ContactDetails, 
                    title: Strings.ContactDetails, 
                    description: $"{Strings.Email}: kuhtin.kirill2017@mail.ru"),

                new ProgramInformationItem(
                    tabHeader: Strings.License, 
                    title: Strings.License, 
                    description: Strings.AllRightsReserved),

                new ProgramInformationItem(
                    tabHeader: Strings.SubjectArea, 
                    title: Strings.SubjectArea, 
                    description: Strings.SubjectAreaDescription),

                new ProgramInformationItem(
                    tabHeader: Strings.Developer, 
                    title: Strings.Developer, 
                    description: Strings.DeveloperDescription),

                new ProgramInformationItem(
                    tabHeader: Strings.SystemRequirements, 
                    title: Strings.MinimumSystemSequirements, 
                    description: Strings.SystemRequirementsDescription),

                new ProgramInformationItem(
                    tabHeader: Strings.Compatibility, 
                    title: Strings.Compatibility, 
                    description: "- pgAdmin 4 v8\r\n- postgreSQL 15")
            };

            if (Items.Count > 0)
            {
                SelectedItem = Items[0];
            }
        }
    }
}
