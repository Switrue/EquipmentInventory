using EquipmentInventory.Properties;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Helpers;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using System.Windows.Controls;
using System.Threading.Tasks;

namespace EquipmentInventory.Forms.Pages;

/// <summary>
/// Логика взаимодействия для UserRegistration.xaml
/// </summary>
public partial class UserRegistration : UserControl
{
    private NotificationService notification;

    public UserRegistration()
    {
        InitializeComponent();
        InitializeUI();
        InitializeParams();
    }

    #region Load

    private void InitializeUI()
    {
        changeImageBtn.Content = Strings.SelectImage;
        titleUserTxtBl.Text = Strings.User;
        HintAssist.SetHint(userFieldTxtB, Strings.Nick);
        HintAssist.SetHint(surnameFieldTxtB, Strings.Surname);
        HintAssist.SetHint(loginFieldTxtB, Strings.Username);
        HintAssist.SetHint(passwordFieldTxtB, Strings.Password);
        createAccountBtn.Content = Strings.CreateAccouont;
        passwordGenerationBtn.Content = Strings.GeneratePassword;
    }

    private void InitializeParams()
    {
        notification = new NotificationService(notificationSnackbar);
    }

    #endregion

    #region Events

    private void Page_MouseDown(object sender, MouseButtonEventArgs e) => ClearFocus();

    private void ChangeImage_Click(object sender, System.Windows.RoutedEventArgs e) => UserAccountService.SelectTheImage(userImage);

    private void ValidBox_TextChanged(object sender, TextChangedEventArgs e) => ValidationHelper.IsTextBoxEmpty((TextBox)sender);

    private void PasswordGeneration_Click(object sender, System.Windows.RoutedEventArgs e) =>
        passwordFieldTxtB.Text = UserAccountService.GetGeneratedPassword();

    #endregion

    #region Registration

    private async void CreateAccount_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (ValidationHelper.AnyTextBoxIsEmpty(textFieldContainer))
        {
            return;
        }

        await UserAccountService.ExecuteTask((Button)sender, Registration);
    }

    private async Task Registration()
    {
        var user = new RegisterRequest
        {
            Username = userFieldTxtB.Text,
            Surname = surnameFieldTxtB.Text,
            Login = loginFieldTxtB.Text,
            Password = passwordFieldTxtB.Text,
            Image = UserAccountService.ConvertImageSourceToBytes(userImage.Source)
        };

        var result = await UsersRequest.UserRegister(user);

        if (result != null)
        {
            ClearPage();
            notification.Show(result.Message);
        }
    }

    private void ClearPage()
    {
        TextFieldHelper.ClearAllText(textFieldContainer);
        ClearFocus();
        UserAccountService.SelectTheDefaultImage(userImage);
    }

    private void ClearFocus()
    {
        Focus();
        TextFieldHelper.ClearAllTextFields(textFieldContainer);
    }

    #endregion
}
