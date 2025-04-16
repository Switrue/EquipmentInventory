using EquipmentInventory.Properties;
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
}
