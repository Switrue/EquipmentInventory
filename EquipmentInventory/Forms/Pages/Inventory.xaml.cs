using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;

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

        private void Page_MouseDown(object sender, MouseButtonEventArgs e) => toggleGridBtn.Focus();

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

        private void Button_Click(object sender, RoutedEventArgs e) => TriggerANotification("Ошибка...");

        private void ExpendFilters_Click(Object sender, RoutedEventArgs e) => ExpendFilters(bool.TryParse(((Button)sender).Tag as string, out bool isExpended));

        private void ExpendFilters(bool isExpended)
        {
            foreach(var child in filterContainer.Children)
            {
                if (child != null && child is Expander)
                {
                    var expender = child as Expander;
                    expender.IsExpanded = isExpended;
                }
            }
        }

        private void TriggerANotification(string message)
        {
            if (NotificationSnackbar.MessageQueue is { } messageQueue && NotificationSnackbar.Message == null)
            {
                Task task = Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            }
        }
    }
}
