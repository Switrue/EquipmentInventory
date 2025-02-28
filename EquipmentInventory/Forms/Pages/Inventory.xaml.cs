using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Pages
{
    /// <summary>
    /// Логика взаимодействия для Inventory.xaml
    /// </summary>
    public partial class Inventory : Page
    {

        public Inventory()
        {
            InitializeComponent();
        }

        private void toggleGridBtn_Click(object sender, RoutedEventArgs e)
        {
            bool isCollapsed = filterAreaGrid.Visibility == Visibility.Collapsed;

            filterAreaGrid.Visibility = isCollapsed ? Visibility.Visible : Visibility.Collapsed;

            var icon = new PackIcon
            {
                Kind = isCollapsed ? PackIconKind.ArrowExpandLeft : PackIconKind.ArrowExpandRight
            };

            toggleGridBtn.Content = icon;
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
