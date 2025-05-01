using EquipmentInventory.Properties;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Helpers;

public static class ValidationHelper
{
    private static bool IsFieldEmpty(Control control, string message)
    {
        if (control is TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                TextFieldHelper.SetTextField(textBox, message);
                return true;
            }
            else
            {
                TextFieldHelper.ClearTextField(textBox);
            }
        }
        else if (control is PasswordBox passwordBox)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                TextFieldHelper.SetTextField(passwordBox, message);
                return true;
            }
            else
            {
                TextFieldHelper.ClearTextField(passwordBox);
            }
        }
        return false;
    }

    public static bool IsTextBoxEmpty(TextBox textBox)
        => IsFieldEmpty(textBox, Strings.FieldEmpty);

    public static bool IsPasswordBoxEmpty(PasswordBox passwordBox)
        => IsFieldEmpty(passwordBox, Strings.FieldEmpty);

    public static bool AnyTextBoxIsEmpty(Panel parent)
    {
        TextFieldHelper.ClearAllTextFields(parent);

        foreach (var child in parent.Children)
        {
            if (child is Control control && (control is TextBox || control is PasswordBox))
            {
                if (control.IsEnabled)
                {
                    if (IsFieldEmpty(control, Strings.FieldEmpty))
                    {
                        return true;
                    }
                }
            }
            else if (child is Panel childPanel)
            {
                if (AnyTextBoxIsEmpty(childPanel))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public static bool IsValidPriceInput(TextBox textBox, string inputText)
    {
        Regex regex = new Regex(@"^[0-9]*([.,]?[0-9]*)?$");
        string newText = textBox.Text.Insert(textBox.CaretIndex, inputText);

        newText = newText.Replace(',', '.');

        bool hasDecimalSeparator = newText.Count(c => c == '.') > 1;

        return regex.IsMatch(newText) && !hasDecimalSeparator;
    }

    public static bool IsValidNumber(string inputText)
        => Regex.IsMatch(inputText, @"\d");

    public static bool IsValidDate(string inputText)
        => Regex.IsMatch(inputText, @"[\d.]");
}
