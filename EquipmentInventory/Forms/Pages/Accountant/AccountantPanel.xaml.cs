using EquipmentInventory.Classes.Enums;
using EquipmentInventory.Classes.Interfaces;
using EquipmentInventory.Properties;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EquipmentInventory.Forms.Pages.Accountant
{
    /// <summary>
    /// Логика взаимодействия для AccountantPanel.xaml
    /// </summary>
    public partial class AccountantPanel : Page
    {
        private IMainWindow _parentWindow;

        private Tables tables = new Tables();

        public AccountantPanel(IMainWindow parentWindow)
        {
            InitializeComponent();
            InitializeUI();
            _parentWindow = parentWindow;

            UpdatePage_Click(tablesRb, null);
        }

        private void InitializeUI()
        {
            tablesRb.Content = Strings.Tables;
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
            RadioButton[] elements = { tablesRb };

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
                    case PageTypes.PageType.Tables:
                        actionsPopupRb.IsOpen = false;
                        _parentWindow.ChangeMainFrameContent(tables);
                        break;
                }
            }
        }

        private PageTypes.PageType? GetSelectedPageType(RadioButton radioButton)
        {
            if (radioButton == tablesRb) 
                return PageTypes.PageType.Tables;

            return null;
        }

        private void DisplayTableOptions_Mouse(object sender, MouseEventArgs e)
        {
            actionsPopupRb.IsOpen = !actionsPopupRb.IsOpen;
        }

        private void Archive_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pressed Archive");
        }

        private void Inventory_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Pressed Inventory");
        }

        #endregion
    }
}
