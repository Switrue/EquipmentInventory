using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Validation = EquipmentInventory.Classes.Data.Validation;

namespace EquipmentInventory.Forms.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserProfile.xaml
    /// </summary>
    public partial class UserProfile : Page
    {
        private UserData _user;

        public UserProfile(UserData user)
        {
            InitializeComponent();
            _user = user;
            InitializeUI();
        }

        #region Load

        private void InitializeUI()
        {
            userTitleTxtBl.Text = $"{_user.Surname} {_user.Username}";
            userRoleTxtBl.Text = _user.UserRole;
            imageGrB.Header = Strings.Customization;
            changeImageBtn.Content = Strings.SelectImage;
            dataGrB.Header = Strings.UserData;
            HintAssist.SetHint(usernameTxtB, Strings.Nick);
            HintAssist.SetHint(surnameTxtB, Strings.Surname);
            HintAssist.SetHint(userPasswordTxtB, Strings.Password);
            saveUserDataBtn.Content = Strings.Save;
            cancelSaveUserDataBtn.Content = Strings.Cancel;
        }

        #endregion

        #region Events

        private void Page_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => ClearFocus();

        private void CancelSaveUserData_Click(object sender, System.Windows.RoutedEventArgs e) => DisableTextFields();

        private void ValidBox_TextChanged(object sender, TextChangedEventArgs e) => Validation.IsTextBoxEmpty((TextBox)sender);

        private void EditUsername_Click(object sender, RoutedEventArgs e) => IncludeTextFields(sender, usernameContainer);

        private void EditUserPassword_Click(object sender, RoutedEventArgs e) => IncludeTextFields(sender, userPasswordContainer);

        private void SaveUserData_Click(object sender, System.Windows.RoutedEventArgs e) => SaveUserData();

        #endregion

        #region Methods

        private void SaveUserData()
        {
            if (ValidationSavingUserData()) return;

            bool dialogResult = CustomMessageBoxHelper.Show(Strings.Warning, Strings.ChangeData, true);

            if (dialogResult)
            {
                MessageBox.Show("Изменено");
                DisableTextFields();
            }
        }

        private bool ValidationSavingUserData()
        {
            bool isUsernameEditing = !editUsernameBtn.IsEnabled;
            bool isPasswordEditing = !editUserPasswordBtn.IsEnabled;

            if (!isUsernameEditing && !isPasswordEditing) return true;

            bool hasValidationErrors =
                (isUsernameEditing && Validation.AnyTextBoxIsEmpty(usernameContainer)) ||
                (isPasswordEditing && Validation.AnyTextBoxIsEmpty(userPasswordContainer));

            if (hasValidationErrors) return true;

            return false;
        }

        private void ClearFocus()
        {
            Keyboard.ClearFocus();
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
                    Validation.IsTextBoxEmpty(textBox);
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
}
