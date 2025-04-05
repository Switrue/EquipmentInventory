namespace EquipmentInventory.Classes.API.Models;

public class User
{
    public long Id { get; set; }

    public string Username { get; set; }

    public string? Description { get; set; }

    public string Role { get; set; }
}
