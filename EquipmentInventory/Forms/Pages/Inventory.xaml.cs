using EquipmentInventory.Classes.Helper;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.Windows.Media.Animation;
using System;

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

        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
        {
            toggleGridBtn.Focus();
        }

        private void toggleGridBtn_Click(object sender, RoutedEventArgs e)
        {
            var toggle = sender as ToggleButton;

            if (toggle != null)
            {
                if (toggle.IsChecked == true)
                {
                    DrawerHost.OpenDrawerCommand.Execute(null, null);
                }
                else
                {
                    DrawerHost.CloseDrawerCommand.Execute(null, null);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (errorTxtBl.Visibility == Visibility.Collapsed)
            {
                errorTxtBl.Visibility = Visibility.Visible;
                errorTxtBl.BeginAnimation(UIElement.OpacityProperty, null);
                Storyboard fadeIn = (Storyboard)FindResource("FadeInStoryboard");
                fadeIn.Begin();
            }
            else
            {
                Storyboard fadeOut = (Storyboard)FindResource("FadeOutStoryboard");
                fadeOut.Completed += (s, args) => errorTxtBl.Visibility = Visibility.Collapsed;
                fadeOut.Begin();
            }
        }
    }
}
