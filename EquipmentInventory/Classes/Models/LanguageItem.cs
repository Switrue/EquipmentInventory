namespace EquipmentInventory.Classes.Models;

public class LanguageItem
{
    public string Name { get; set; }
    public string Code { get; set; }

    public LanguageItem(string name, string code)
    {
        Name = name;
        Code = code;
    }
}
