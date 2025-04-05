using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Data;

public static class Validation
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
    {
        return IsFieldEmpty(textBox, Strings.FieldEmpty);
    }

    public static bool IsPasswordBoxEmpty(PasswordBox passwordBox)
    {
        return IsFieldEmpty(passwordBox, Strings.FieldEmpty);
    }

    public static bool AnyTextBoxIsEmpty(Panel parent)
    {
        TextFieldHelper.ClearAllTextFields(parent);

        foreach (var child in parent.Children)
        {
            if (child is TextBox textBox)
            {
                if (!textBox.IsEnabled)
                {
                    continue;
                }

                if (IsFieldEmpty(textBox, Strings.FieldEmpty))
                {
                    return true;
                }
            }
            else if (child is Panel panel)
            {
                if (AnyTextBoxIsEmpty(panel))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
