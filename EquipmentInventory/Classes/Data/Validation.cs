using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using System.Linq;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Data
{
    public static class Validation
    {
        private static bool IsFieldEmpty(Control control, string message)
        {
            if (control is TextBox textBox )
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
            return parent.Children.OfType<Control>().Any(child => IsFieldEmpty(child, Strings.FieldEmpty));
        }
    }
}
