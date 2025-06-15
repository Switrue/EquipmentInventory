namespace EquipmentInventory.API.Helpers;

public static class ExceptionExtensions
{
    public static string GetFullMessage(this Exception ex)
    {
        return ex.InnerException == null
            ? ex.Message
            : $"{ex.Message} -> {ex.InnerException.GetFullMessage()}";
    }

    public static string Center(this string text, int width)
    {
        if (text.Length >= width) return text;

        int padding = (width - text.Length) / 2;
        return text.PadLeft(text.Length + padding).PadRight(width);
    }
}
