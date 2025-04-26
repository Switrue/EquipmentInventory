namespace EquipmentInventory.Classes.Data.Models;

public class MemberDto
{
    public long Id { get; set; }
    public string Surname { get; set; }
    public string Username { get; set; }
    public string Position { get; set; }

    public string FullName => $"{Surname} {Username}";
}
