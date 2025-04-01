using EquipmentInventory.Properties;
using System.Collections.Generic;
using System.Linq;

namespace EquipmentInventory.Classes.Models.ViewModels
{
    public class ComboBoxesViewModel
    {
        private LanguageItem _selectedLanguage;

        public IList<int> LongIntegerList { get; }
        public List<LanguageItem> Languages { get; }
        public LanguageItem SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (_selectedLanguage != value)
                {
                    _selectedLanguage = value;
                    ApplyLanguage();
                }
            }
        }

        public ComboBoxesViewModel()
        {
            LongIntegerList = new List<int>(Enumerable.Range(1, 100));

            Languages = new List<LanguageItem>()
            {
                new LanguageItem(
                    name: "Русский", 
                    code: "ru-RU"),

                new LanguageItem(
                    name: "English", 
                    code: "en-US")
            };

            var savedLang = Settings.Default.CultureInfo;
            SelectedLanguage = Languages.FirstOrDefault(l => l.Code == savedLang) ?? Languages[0];
        }

        private void ApplyLanguage() => Settings.Default.CultureInfo = SelectedLanguage.Code;
    }
}
