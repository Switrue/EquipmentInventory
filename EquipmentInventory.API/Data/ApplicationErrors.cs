namespace EquipmentInventory.API.Data;

public static class ApplicationErrors
{
    /// <summary>
    /// Глобальные настройки
    /// </summary>
    public const string InvalidCode = "Код недействителен";
    public const string AdminPasswordReset = "Пароль администратора сброшен";

    /// <summary>
    /// Авторизация и регистрация
    /// </summary>
    public const string InvalidLoginOrPassword = "Неверный логин или пароль";
    public const string LoginAlreadyInUse = "Логин уже используется";
    public const string RoleNotFound = "Роль по умолчанию не найдена";
    public const string IncorrectPasswordLength = "Пароль не может быть меньше 6 символов";

    /// <summary>
    /// Пользователь
    /// </summary>
    public const string UserNotFound = "Пользователь не найден";
    public const string UserCreated = "Пользователь успешно создан";

    /// <summary>
    /// Сегмент, связанный с базой данных
    /// </summary>
    public const string CreationError = "Ошибка при создании";
    public const string UpdateError = "Ошибка при обновлении";
    public const string SuccessfullyUpdated = "Данные обновлены";

    /// <summary>
    /// Серверная часть приложения
    /// </summary>
    public const string ServerError = "Внутренняя ошибка сервера";
    public const string AuthenticationError = "Ошибка аутентификации";
    public const string EmptyError = "Контент оказался пуст";
    public const string Success = "Успех";
}
