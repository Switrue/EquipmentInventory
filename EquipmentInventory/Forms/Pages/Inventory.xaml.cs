using EquipmentInventory.Classes.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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

        private async void toggleGridBtn_Click(object sender, RoutedEventArgs e)
        {
            AnimationHelper.ToggleSlideAnimation(filterAreaGrid);
            await AnimationHelper.AnimatedBlock((ToggleButton)sender);
        }

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
        }
    }
}
