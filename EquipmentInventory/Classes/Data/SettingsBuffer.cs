using EquipmentInventory.Properties;

namespace EquipmentInventory.Classes.Data;

public class SettingsBuffer
{
    public string CultureInfo { get; set; }
    public int YearOfObsolescence { get; set; }

    public SettingsBuffer()
    {
        CultureInfo = Settings.Default.CultureInfo;
        YearOfObsolescence = Settings.Default.YearOfObsolescence;
    }

    public void ApplyToYearOfObsolescence()
    {
        Settings.Default.YearOfObsolescence = YearOfObsolescence;
        Save();
    }

    public void ApplyToCultureInfo()
    {
        Settings.Default.CultureInfo = CultureInfo;
        Save();
    }

    private void Save() => Settings.Default.Save();
}
