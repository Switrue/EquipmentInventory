using System.Windows.Controls;

namespace EquipmentInventory.Classes.Models
{
    public class SettingItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public UserControl Page { get; set; }

        public SettingItem(string title, string icon, UserControl page)
        {
            Title = title;
            Icon = icon;
            Page = page;
        }
    }
}
