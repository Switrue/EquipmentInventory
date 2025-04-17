namespace EquipmentInventory.API.Helpers;

public class HashPasswordHelper
{
    public bool VerifyPassword(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
}
