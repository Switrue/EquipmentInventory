using System.Windows.Controls;

namespace EquipmentInventory.Classes.Models
{
    public class SettingItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public Page Page { get; set; }

        public SettingItem(string title, string icon, Page page)
        {
            Title = title;
            Icon = icon;
            Page = page;
        }
    }
}
