using EquipmentInventory.Classes.Services;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System.Windows.Controls;
using System.Windows.Input;

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

        private void InitializeUI()
        {
            changeImageBtn.Content = Strings.SelectImage;
            titleUserTxtBl.Text = Strings.User;
            HintAssist.SetHint(userFieldTxtB, Strings.Nick);
            HintAssist.SetHint(surnameFieldTxtB, Strings.Surname);
            HintAssist.SetHint(loginFieldTxtB, Strings.Username);
            HintAssist.SetHint(passwordFieldTxtB, Strings.Password);
            createAccountBtn.Content = Strings.CreateAccouont;
            passwordGenerationBtn.Content = Strings.ChangePassword;
            hidePasswordTglBtn.ToolTip = Strings.HidePassword;
        }

        private void InitializeParams()
        {
            notification = new NotificationService(notificationSnackbar);
        }

        private void Page_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => Keyboard.ClearFocus();

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e) => notification.Show("Зарегестрирован");
    }
}
