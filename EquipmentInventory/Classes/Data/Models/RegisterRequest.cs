namespace EquipmentInventory.Classes.Data.Models;

public class RegisterRequest
{
    public string Username { get; set; }
    public string Surname { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public byte[]? Image { get; set; }
}
