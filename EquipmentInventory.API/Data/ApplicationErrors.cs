namespace EquipmentInventory.API.Data;

public static class ApplicationErrors
{
    /// <summary>
    /// Глобальные настройки
    /// </summary>
    public const string InvalidCode = "Код недействителен";
    public const string AdminPasswordReset = "Пароль администратора сброшен";
    public const string ArchiveReset = "Архив сброшен";

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
    /// Техника
    /// </summary>
    public const string UniqueNumber = "Номер должен быть уникальным";
    public const string UniqueComputer = "Компьютер уже привязан к другой технике";
    public const string IncorrectTechniqueType = "Неверный тип техники";
    public const string IncorrectSupplier = "Неверный поставщик";
    public const string EmployeeNotFound = "Сотрудник не найден";
    public const string OfficeNotFound = "Кабинет не найден";
    public const string ComputerNotFound = "Компьютер не найден";
    public const string FiltrationRestriction = "Только один фильтр за раз";

    /// <summary>
    /// Сотрудники
    /// </summary>
    public const string IncorrectPosition = "Неправильная должность";

    /// <summary>
    /// Сегмент, связанный с базой данных
    /// </summary>
    public const string CreationError = "Ошибка при создании";
    public const string UpdateError = "Ошибка сохранения данных";
    public const string EmptyError = "Контент оказался пуст";
    public const string NotFound = "Результат не найден";
    public const string SuccessfullyUpdated = "Данные обновлены";
    public const string SuccessfullyAdded = "Успешно добавлено";
    public const string DeleteSuccessfully = "Успешно удалено";
    public const string InternalServerError = "Внутренняя ошибка сервера";
    public const string Success = "Успех";

    /// <summary>
    /// Общая валидация
    /// </summary>
    public const string InvalidPage = "Номер страницы должен быть положительным";
    public const string InvalidPageSize = "Размер страницы должен быть от 1 до 100";
    public const string InvalidModelType = "Неверный тип модели";
    public const string UniqueName = "Название должно быть уникальным";
    public const string IncorrectFormat = "Неверный формат";
    public const string ViolatesTheRulesOfAddiction = "Нарушает правила зависимости";

    /// <summary>
    /// Серверная часть приложения
    /// </summary>
    public const string ServerError = "Внутренняя ошибка сервера";
    public const string Unauthorized = "Требуется авторизация";
    public const string Forbidden = "Доступ запрещен";
    public const string AuthenticationError = "Ошибка аутентификации";
    public const string Errors = "Произошла одна или несколько ошибок проверки";
}
