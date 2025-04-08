using EquipmentInventory.Properties;
using System;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using EquipmentInventory.Classes.Helpers;
using System.Windows.Controls;
using Validation = EquipmentInventory.Classes.Data.Validation;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Classes.Models.ViewModels;
using EquipmentInventory.Classes.Handlers;
using EquipmentInventory.Classes.Data;
using EquipmentInventory.Forms.Pages.Admin;

namespace EquipmentInventory.Forms.Windows;

/// <summary>
/// Логика взаимодействия для AuthoUser.xaml
/// </summary>
public partial class AuthoUser : Window
{
    public AuthoUser()
    {
        InitializeComponent();
        InitializeUI();
        InitializeParams();
    }

    #region Load

    private void InitializeUI()
    {
        windowTitle.Text = Strings.AuthoTitle;
        Title = windowTitle.Text;
        loginBtn.Content = Strings.SignIn;
        collapseBtn.ToolTip = Strings.Collapse;
        closeBtn.ToolTip = Strings.Close;
        rememberUserChB.Content = Strings.RememberUser;
        HintAssist.SetHint(usernameTextB, Strings.Username);
        HintAssist.SetHint(passwordPsB, Strings.Password);
    }

    private void InitializeParams()
    {
        DataContext = new WindowManagementViewModel(new WindowService(this, new WindowStateHandler()));
    }

    #endregion

    #region Window management

    private void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Keyboard.ClearFocus();
        TextFieldHelper.ClearAllTextFields(textFieldContainer);
    }

    #endregion

    #region Authorization

    private void Login_Click(object sender, EventArgs e)
    {
        if (Validation.AnyTextBoxIsEmpty(textFieldContainer))
        {
            return;
        }

        var window = new MainWindow(new UserData(1));
        var panel = new UserPanel(window, Classes.Enums.TabType.Tables, true);
        window.ChangeControlPanelFrameContent(panel);
        window.Show();
        Close();
    }

    #endregion

    #region Valid changed

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => Validation.IsTextBoxEmpty((TextBox)sender);

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) => Validation.IsPasswordBoxEmpty((PasswordBox)sender);

    #endregion
}
