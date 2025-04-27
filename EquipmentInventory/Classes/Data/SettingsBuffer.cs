using EquipmentInventory.Properties;

namespace EquipmentInventory.Classes.Data;

public class SettingsBuffer
{
    public string CultureInfo { get; set; }
    public int YearOfObsolescence { get; set; }
    public int NumberOfRecords { get; set; }

    public SettingsBuffer()
    {
        CultureInfo = Settings.Default.CultureInfo;
        YearOfObsolescence = Settings.Default.YearOfObsolescence; 
        NumberOfRecords = Settings.Default.PageSize;
    }

    public void ApplyNumberOfRecords()
    {
        Settings.Default.PageSize = NumberOfRecords;
        Save();
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
