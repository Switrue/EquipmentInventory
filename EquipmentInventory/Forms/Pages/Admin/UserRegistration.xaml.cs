using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows.Input;
using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Helpers;
using System.Windows.Controls;
using Validation = EquipmentInventory.Classes.Data.Validation;

namespace EquipmentInventory.Forms.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserRegistration.xaml
    /// </summary>
    public partial class UserRegistration : Page
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

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            TextFieldHelper.ClearAllTextFields(textFieldContainer);
        }

        private void PasswordGeneration_Click(object sender, System.Windows.RoutedEventArgs e) => passwordFieldTxtB.Text = PasswordGenerator.Get();

        private void CreateAccount_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var container = textFieldContainer;

            TextFieldHelper.ClearAllTextFields(container);

            if (Validation.AnyTextBoxIsEmpty(container))
            {
                return;
            }

            notification.Show("Зарегестрирован");
        }
    }
}
