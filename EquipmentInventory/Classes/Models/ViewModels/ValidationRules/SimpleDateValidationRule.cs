using System.Globalization;
using System;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Models.ViewModels.ValidationRules;

public class SimpleDateValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        return DateTime.TryParse((value ?? "").ToString(),
            CultureInfo.CurrentCulture,
            DateTimeStyles.AssumeLocal | DateTimeStyles.AllowWhiteSpaces,
            out _)
            ? ValidationResult.ValidResult
            : new ValidationResult(false, "Invalid date");
    }
}
