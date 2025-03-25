using EquipmentInventory.Classes.Services;
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
            InitializeParams();
        }

        private void InitializeParams()
        {
            notification = new NotificationService(notificationSnackbar);
        }

        private void Page_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e) => Keyboard.ClearFocus();

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e) => notification.Show("Зарегестрирован");
    }
}
