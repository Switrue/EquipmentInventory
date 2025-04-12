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

namespace EquipmentInventory.Forms.Pages;

/// <summary>
/// Логика взаимодействия для UserProfile.xaml
/// </summary>
public partial class UserProfile : UserControl
{
    private Users _user;
    private bool isUsernameEditing;
    private bool isPasswordEditing;

    public UserProfile(Users user)
    {
        InitializeComponent();
        _user = user;
        InitializeUI();
    }

    #region Load

    private void InitializeUI()
    {
        userTitleTxtBl.Text = $"{_user.Surname} {_user.Username}";
        userRoleTxtBl.Text = _user.Role;
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

        UserAccountService.SetImageSource(_user.Image, userImage);
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
        if (ValidationSavingUserData()) return;

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
                CustomMessageBoxHelper.Show(result);
            }
        }
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
        ToggleTextBoxEnabledStateInPanel(container, true);
    }

    private void DisableTextFields()
    {
        editBtnContainer.Children.OfType<Button>().All(b => b.IsEnabled = true);
        ToggleTextBoxEnabledStateInPanel(userDataContainer, false);
    }

    private void ToggleTextBoxEnabledStateInPanel(Panel parent, bool isEnabled)
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

        ClearFocus();
    }

    #endregion
}
