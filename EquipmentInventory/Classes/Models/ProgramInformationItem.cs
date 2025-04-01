namespace EquipmentInventory.Classes.Models
{
    public class ProgramInformationItem
    {
        public string TabHeader { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ProgramInformationItem(string tabHeader, string title, string description)
        {
            TabHeader = tabHeader;
            Title = title;
            Description = description;
        }
    }
}
