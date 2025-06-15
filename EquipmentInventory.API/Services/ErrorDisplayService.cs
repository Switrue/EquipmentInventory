using EquipmentInventory.API.Data.Enums;
using EquipmentInventory.API.Helpers;

namespace EquipmentInventory.API.Services;

public static class ErrorDisplayService
{
    private const ConsoleColor HeaderColor = ConsoleColor.Red;
    private const ConsoleColor FooterColor = ConsoleColor.Yellow;
    private const string DefaultLogFile = "error.log";
    private const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    public static void ShowError(Exception exception, ErrorType errorType = ErrorType.Configuration)
    {
        try
        {
            PrintErrorHeader(errorType);
            Console.Error.WriteLine(exception.Message);

            WriteErrorBody(exception);
            WaitForUserInput();

            LogErrorToFile(exception);
        }
        finally
        {
            Console.ResetColor();
        }
    }

    private static void PrintErrorHeader(ErrorType errorType)
    {
        Console.ForegroundColor = HeaderColor;

        var headerText = errorType switch
        {
            ErrorType.Configuration => "ОШИБКА КОНФИГУРАЦИИ",
            ErrorType.Startup => "ОШИБКА ЗАПУСКА",
            ErrorType.Runtime => "КРИТИЧЕСКАЯ ОШИБКА",
            _ => "НЕИЗВЕСТНАЯ ОШИБКА"
        };

        Console.Error.WriteLine("╔════════════════════════════════════════╗");
        Console.Error.WriteLine($"║{headerText.Center(40)}║");
        Console.Error.WriteLine("╚════════════════════════════════════════╝");
        Console.Error.WriteLine();
    }

    private static void WriteErrorBody(Exception exception)
    {
        Console.ResetColor();
        Console.Error.WriteLine("\nПодробности:");
        Console.Error.WriteLine(exception.GetFullMessage());
        Console.Error.WriteLine("\nStack Trace:");
        Console.Error.WriteLine(exception.StackTrace);
    }

    private static void WaitForUserInput()
    {
        Console.ForegroundColor = FooterColor;
        Console.Error.WriteLine("\nНажмите любую клавишу для завершения...");
        Console.ReadKey();
        Environment.ExitCode = 1;
    }

    public static void LogErrorToFile(Exception exception, string filePath = DefaultLogFile)
    {
        try
        {
            File.AppendAllText(filePath,
                $"[{DateTime.Now.ToString(DateTimeFormat)}] ERROR:\n" +
                $"Message: {exception.GetFullMessage()}\n" +
                $"Type: {exception.GetType().Name}\n" +
                $"Stack Trace:\n{exception.StackTrace}\n\n");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Ошибка логирования: {ex.Message}");
        }
    }
}
