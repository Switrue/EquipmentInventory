namespace EquipmentInventory.Classes.Data.Models;

public class Users
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Surname { get; set; }
    public byte[]? Image { get; set; }
    public string Role { get; set; }
}
