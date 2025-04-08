namespace EquipmentInventory.Classes.Data;

public class UserData
{
    public long UserId { get; private set; }
    public string UserRole { get; private set; } = "Системный администратор";
    public string Username { get; private set; } = "Олег";
    public string Surname { get; private set; } = "Балтийский";

    public UserData(long userId)
    {
        UserId = userId;
    }
}
