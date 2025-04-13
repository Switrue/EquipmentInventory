using EquipmentInventory.Classes.Services;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EquipmentInventory.Classes.Data.Requests;
using System.Threading.Tasks;
using EquipmentInventory.Classes.Data.Interfaces;

namespace EquipmentInventory.Forms.Pages;

/// <summary>
/// Логика взаимодействия для UserProfile.xaml
/// </summary>
public partial class UserProfile : UserControl
{
    private IMainWindow _mainWindow;
    private NotificationService notification;
    private bool isUsernameEditing;
    private bool isPasswordEditing;

    public UserProfile(IMainWindow mainWindow)
    {
        InitializeComponent();
        _mainWindow = mainWindow;
        InitializeUI();
        InitializeParams();
    }

    #region Load

    private void InitializeUI()
    {
        SetUserInfo();

        UserAccountService.SetImageSource(App.user.Image, userImage);

        imageGrB.Header = Strings.Customization;
        changeImageBtn.Content = Strings.SelectImage;
        dataGrB.Header = Strings.UserData;
        HintAssist.SetHint(usernameTxtB, Strings.Nick);
        HintAssist.SetHint(surnameTxtB, Strings.Surname);
        HintAssist.SetHint(userPasswordTxtB, Strings.Password);
        editUsernameBtn.ToolTip = Strings.Edit;
        editUserPasswordBtn.ToolTip = Strings.Edit;
        saveUserDataBtn.Content = Strings.Save;
        saveUserImage.Content = Strings.Save;
        cancelSaveUserDataBtn.Content = Strings.Cancel;
    }

    private void InitializeParams()
    {
        notification = new NotificationService(notificationSnackbar);
    }

    private void SetUserInfo()
    {
        string username = $"{App.user.Surname} {App.user.Username}".Trim();
        userTitleTxtBl.Text = string.IsNullOrWhiteSpace(username) ? Strings.DefaultUserName : username;
        userRoleTxtBl.Text = App.user.Role ?? Strings.DefaultRole;
    }

    #endregion

    #region Events

    private void Page_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => ClearFocus();

    private void CancelSaveUserData_Click(object sender, System.Windows.RoutedEventArgs e) => DisableTextFields();

    private void ValidBox_TextChanged(object sender, TextChangedEventArgs e) => ValidationHelper.IsTextBoxEmpty((TextBox)sender);

    private void EditUsername_Click(object sender, RoutedEventArgs e) => IncludeTextFields(sender, usernameContainer);

    private void EditUserPassword_Click(object sender, RoutedEventArgs e) => IncludeTextFields(sender, userPasswordContainer);

    private void ChangeImage_Click(object sender, RoutedEventArgs e) => UserAccountService.SelectTheImage(userImage);

    private async void SaveUserData_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        if (ValidationSavingUserData())
        {
            return;
        }

        bool dialogResult = CustomMessageBoxHelper.Show(Strings.Warning, Strings.ChangeData, true);

        if (dialogResult)
        {
            var userUpdate = new RegisterRequest();

            if (!isUsernameEditing)
            {
                userUpdate.Username = usernameTxtB.Text;
                userUpdate.Surname = surnameTxtB.Text;
            }

            if (!isPasswordEditing)
            {
                userUpdate.Password = userPasswordTxtB.Text;
            }

            DisableTextFields();

            await UserAccountService.ExecuteTask(
                (Button)sender, 
                async () => { await Update(userUpdate); });
        }
    }

    private async void SaveUserImage_Click(object sender, RoutedEventArgs e)
    {
        var userUpdate = new RegisterRequest
        {
            Image = UserAccountService.ConvertImageSourceToBytes(userImage.Source)
        };

        await UserAccountService.ExecuteTask(
            (Button)sender,
            async () => { await Update(userUpdate); });
    }

    #endregion

    #region Methods

    private async Task Update(RegisterRequest userUpdate)
    {
        var result = await UserRequest.UpdateUserProfile(userUpdate);

        if (result != null)
        {
            var updateJwt = await AuthoRequest.UpdateJwtToken();

            if (updateJwt != null)
            {
                await AuthorizationService.SetUser(updateJwt);

                SetUserInfo();
                UpdateMainWindow();

                notification.Show(result);
            }
        }
    }

    private void UpdateMainWindow()
    {
        _mainWindow.SetUsernameOnTheMainWindow();
        _mainWindow.SetUserImageOnTgeMainWindow();
    }

    private bool ValidationSavingUserData()
    {
        isUsernameEditing = editUsernameBtn.IsEnabled;
        isPasswordEditing = editUserPasswordBtn.IsEnabled;

        if (isUsernameEditing && isPasswordEditing) return true;

        if (ValidationHelper.AnyTextBoxIsEmpty(userDataContainer)) return true;

        return false;
    }

    private void ClearFocus()
    {
        Focus();
        TextFieldHelper.ClearAllTextFields(userDataContainer);
    }

    private void IncludeTextFields(object sender, Panel container)
    {
        ((Button)sender).IsEnabled = false;
        TextFieldHelper.ToggleTextBoxEnabledStateInPanel(container, true);
        ClearFocus();
    }

    private void DisableTextFields()
    {
        editBtnContainer.Children.OfType<Button>().All(b => b.IsEnabled = true);
        TextFieldHelper.ToggleTextBoxEnabledStateInPanel(userDataContainer, false);
        ClearFocus();
    }

    #endregion
}
