using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Helpers;
using System.Windows.Controls;
using Validation = EquipmentInventory.Classes.Data.Validation;
using EquipmentInventory.Classes.Data.Models;
using System.Threading.Tasks;
using EquipmentInventory.Classes.Data.Requests;

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

    private void Page_MouseDown(object sender, MouseButtonEventArgs e) => ClearFocus();

    private void PasswordGeneration_Click(object sender, System.Windows.RoutedEventArgs e) => 
        passwordFieldTxtB.Text = UserAccount.GetGeneratedPassword();

    private async void CreateAccount_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (Validation.AnyTextBoxIsEmpty(textFieldContainer)) return;

        var btn = (Button)sender;

        btn.IsEnabled = false;

        try
        {
            await Registration();
        }
        finally
        {
            btn.IsEnabled = true;
        }
    }

    private async Task Registration()
    {
        var user = new RegisterRequest
        {
            Username = userFieldTxtB.Text,
            Surname = surnameFieldTxtB.Text,
            Login = loginFieldTxtB.Text,
            Password = passwordFieldTxtB.Text,
            Image = UserAccount.ConvertImageSourceToBytes(userImage.Source)
        };

        var result = await UserRequest.UserRegister(user);

        if (result != null)
        {
            ClearPage();
            notification.Show(result);
        }
    }

    private void ClearPage()
    {
        TextFieldHelper.ClearAllText(textFieldContainer);
        ClearFocus();
        UserAccount.SelectTheDefaultImage(userImage);
    }

    private void ClearFocus()
    {
        Focus();
        TextFieldHelper.ClearAllTextFields(textFieldContainer);
    }

    private void ValidBox_TextChanged(object sender, TextChangedEventArgs e) => Validation.IsTextBoxEmpty((TextBox)sender);

    private void ChangeImage_Click(object sender, System.Windows.RoutedEventArgs e) => UserAccount.SelectTheImage(userImage);
}
