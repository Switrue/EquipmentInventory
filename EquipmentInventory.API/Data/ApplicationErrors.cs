namespace EquipmentInventory.API.Data;

public static class ApplicationErrors
{
    /// <summary>
    /// Авторизация
    /// </summary>
    public const string InvalidLoginOrPassword = "Неверный логин или пароль";

    /// <summary>
    /// Пользователь
    /// </summary>
    public const string UserNotFound = "Пользователь не найден";
    public const string UserCreated = "Пользователь успешно создан";

    public const string CreationError = "Ошибка при создании";
}
