using System;
using System.Linq;

namespace EquipmentInventory.Classes.Helpers;

public class CaptchaGenerator
{
    private static readonly Random random = new Random();
    private const string Chars = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";

    public string GeneratedText { get; private set; }
    public string UserInput { get; set; }

    public void GenerateNew(int length = 6)
    {
        GeneratedText = new string(Enumerable.Repeat(Chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    public bool Validate()
        => string.Equals(GeneratedText, UserInput, StringComparison.OrdinalIgnoreCase);
}
