using EquipmentInventory.Classes.Enums;
using EquipmentInventory.Classes.Interfaces;
using EquipmentInventory.Properties;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EquipmentInventory.Forms.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page, IMainPanel
    {
        private IMainWindow _parentWindow;

        private UserRegistration userRegistration = new UserRegistration();

        private Dictionaries dictionaries = new Dictionaries();

        private Tables tables;

        public AdminPanel(IMainWindow parentWindow)
        {
            InitializeComponent();
            InitializeUI();
            _parentWindow = parentWindow;
            tables = new Tables(this);
            UpdatePage_Click(tablesRb, null);
        }

        private void InitializeUI()
        {
            userRegistrationRb.Content = Strings.UserRegistration.ToUpper();
            tablesRb.Content = Strings.Tables.ToUpper();
            dictionariesRb.Content = Strings.Dictionaries.ToUpper();
            inventoryBtn.Content = Strings.Inventory;
            archiveBtn.Content = Strings.Archive;
        }

        #region Control panel

        private void ColorChange_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                SolidColorBrush textColor = (SolidColorBrush)Application.Current.Resources["TextColor"];
                SolidColorBrush secondaryColor = (SolidColorBrush)Application.Current.Resources["SecondaryColor"];

                UpdateRadioButtonColors(textColor);

                radioButton.Foreground = secondaryColor;
            }
        }

        private void UpdateRadioButtonColors(SolidColorBrush textColor)
        {
            RadioButton[] elements = { userRegistrationRb, tablesRb, dictionariesRb };

            foreach (var element in elements)
            {
                if (element != null)
                {
                    element.Foreground = textColor;
                }
            }
        }

        private void UpdatePage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton)
            {
                var selectedPage = GetSelectedPageType(radioButton);

                switch (selectedPage)
                {
                    case PageTypes.PageType.UserRegistration:
                        _parentWindow.ChangeMainFrameContent(userRegistration);
                        break;
                    case PageTypes.PageType.Tables:
                        actionsPopupRb.IsOpen = false;
                        _parentWindow.ChangeMainFrameContent(tables);
                        break;
                    case PageTypes.PageType.Dictionaries:
                        _parentWindow.ChangeMainFrameContent(dictionaries);
                        break;
                }
            }
        }

        private PageTypes.PageType? GetSelectedPageType(RadioButton radioButton)
        {
            if (radioButton == userRegistrationRb)
                return PageTypes.PageType.UserRegistration;
            else if (radioButton == tablesRb)
                return PageTypes.PageType.Tables;
            else if (radioButton == dictionariesRb)
                return PageTypes.PageType.Dictionaries;

            return null;
        }

        private void DisplayTableOptions_Mouse(object sender, MouseEventArgs e) => actionsPopupRb.IsOpen = !actionsPopupRb.IsOpen;

        public void Archive_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Pressed Archive from Adm");

        public void Inventory_Click(object sender, RoutedEventArgs e) => _parentWindow.ChangeMainFrameContent(new Inventory());

        #endregion
    }
}
