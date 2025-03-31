namespace EquipmentInventory.Classes.Models
{
    public class SettingModel
    {
        public string Parameter { get; set; }
        public string Icon { get; set; }

        public SettingModel(string parameter, string icon)
        {
            Parameter = parameter;
            Icon = icon;
        }
    }
}
