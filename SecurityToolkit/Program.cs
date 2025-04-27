using SecurityToolkit.Data;

namespace SecurityToolkit;

class Program
{
    /// <summary>
    /// Генерация данных для серверной части приложения
    /// </summary>
    static void Main(string[] args)
    {
        DataGeneration();
    }

    #region Methods
    private static void DataGeneration()
    {
        while (true)
        {
            Console.Write("Сгенерировать? (y/n): ");
            var result = Console.ReadLine();
            Console.Clear();

            if (result == "y")
            {
                FormattedConclusion();
            }
            else if (result == "n")
            {
                return; 
            }
            else
            {
                Console.WriteLine("Некорректный ввод. Пожалуйста, введите 'y' для генерации или 'n' для выхода.\r\n");
            }
        }
    }

    private static void FormattedConclusion()
    {
        var countCodes = GetCountOfCodes();

        WriteKey();
        Console.WriteLine();
        WriteCodes(countCodes);
        Console.WriteLine();
    }

    private static void WriteKey()
    {
        Console.WriteLine("Ключ:");

        var key = GeneratedHelper.GenerateRandomKey();
        Console.WriteLine(key);
    }

    private static void WriteCodes(int count)
    {
        Console.WriteLine("Коды:");

        for (var i = 0; i < count; i++)
        {
            var code = GeneratedHelper.GenerateRandomCode();
            Console.WriteLine(code);
        }
    }

    private static int GetCountOfCodes()
    {
        while (true)
        {
            Console.Write("Количество генерируемых кодов: ");

            if (!int.TryParse(Console.ReadLine(), out int value) || value <= 0 || value > 100)
            {
                Console.Clear();
                Console.WriteLine("Введите целое число от 1 до 100.\r\n");
            }
            else
            {
                Console.Clear();
                return value;
            }
        }
    }
    #endregion
}
