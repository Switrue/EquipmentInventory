using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EquipmentInventory.Classes.Helpers;

public static class TextFieldHelper
{
    public static void ResetTextField(Control element, string message, SolidColorBrush highlightСolor)
    {
        SolidColorBrush textColor = (SolidColorBrush)Application.Current.Resources["TextColor"];
        element.Foreground = message == string.Empty ? textColor : highlightСolor;
        HintAssist.SetForeground(element, highlightСolor);
        TextFieldAssist.SetUnderlineBrush(element, highlightСolor);
        HintAssist.SetHelperText(element, message);
    }   

    public static void SetTextField(Control element, string message)
    {
        SolidColorBrush accentColor = (SolidColorBrush)Application.Current.Resources["AccentColor"];
        ResetTextField(element, message, accentColor);
    }

    public static void SetTextField(Control element)
    {
        SolidColorBrush blueGreyColor = (SolidColorBrush)Application.Current.Resources["PrimaryColor"];
        ResetTextField(element, string.Empty, blueGreyColor);
    }

    public static void ClearTextField(Control element)
    {
        SetTextField(element);
    }

    public static void ClearText(Control control)
    {
        if (control is TextBox textBox)
        {
            textBox.Text = string.Empty;
        }
        else if (control is PasswordBox passwordBox)
        {
            passwordBox.Password = string.Empty;
        }
    }

    public static void ClearAllTextFields(Panel parent)
    {
        foreach (var child in parent.Children)
        {
            if (child is null) continue;

            if (child is Control control && (control is TextBox || control is PasswordBox))
            {
                SetTextField(control);
            }
            else if (child is Panel panel)
            {
                ClearAllTextFields(panel);
            }
        }
    }

    public static void ClearAllText(Panel parent)
    {
        foreach (var child in parent.Children)
        {
            if (child is null) continue;

            if (child is Control control && (control is TextBox || control is PasswordBox)) 
            {
                ClearText(control);
            }
        }
    }

    public static void ToggleTextBoxEnabledStateInPanel(Panel parent, bool isEnabled)
    {
        foreach (var child in parent.Children)
        {
            if (child is null) continue;

            if (child is TextBox textBox)
            {
                ValidationHelper.IsTextBoxEmpty(textBox);
                textBox.Text = string.Empty;
                textBox.IsEnabled = isEnabled;
            }
            else if (child is Panel panel)
            {
                ToggleTextBoxEnabledStateInPanel(panel, isEnabled);
            }
        }
    }
}
