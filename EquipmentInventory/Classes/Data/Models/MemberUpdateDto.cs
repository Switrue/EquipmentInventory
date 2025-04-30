namespace EquipmentInventory.Classes.Data.Models;

public class MemberUpdateDto
{
    public string? Username { get; set; }
    public string? Surname { get; set; }
    public long? IdPosition { get; set; }

    public bool IsValid()
    {
        return !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Surname) &&
               IdPosition.HasValue;
    }
}
