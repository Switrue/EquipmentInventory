using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using EquipmentInventory.Properties;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Handlers;
using EquipmentInventory.Classes.Helper;

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

    private async void Login_Click(object sender, EventArgs e)
    {
        if (ValidationHelper.AnyTextBoxIsEmpty(textFieldContainer))
        {
            return;
        }

        await UserAccountService.ExecuteTask((Button)sender, Autho);
    }

    private async Task Autho()
    {
        try
        {
            var jwt = await AuthoRequest.GetJwtToken(usernameTextB.Text, passwordPsB.Password);

            if (jwt == null) return;

            if (rememberUserChB.IsChecked == true)
            {
                AuthorizationService.SaveJwt(jwt);
            }

            await AuthorizationService.Authorize(jwt);
            Close();
        }
        catch
        {
            CustomMessageBoxHelper.Show(Strings.LoginError, Strings.LoginErrorDescription);
        }
    }

    #endregion

    #region Valid changed

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => ValidationHelper.IsTextBoxEmpty((TextBox)sender);

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) => ValidationHelper.IsPasswordBoxEmpty((PasswordBox)sender);

    #endregion
}
