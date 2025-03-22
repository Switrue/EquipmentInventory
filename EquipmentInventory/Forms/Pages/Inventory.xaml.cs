using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;
using EquipmentInventory.Properties;

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
            InitializeUI();
        }

        #region Load

        private void InitializeUI()
        {
            HintAssist.SetHint(searchTxtB, Strings.Search);
            viewFiltersBtn.Content = Strings.View;
            hidenFiltersBtn.Content = Strings.Hide;
            techniqueExpander.Header = Strings.Technique;
            techNameRBtn.Content = Strings.Name;
            techTypeRBtn.Content = Strings.Type;
            techNumberRBtn.Content = Strings.InventoryNumber;
            techCostRBtn.Content = Strings.Price;
            HintAssist.SetHint(costFromTxtB, Strings.CostFrom);
            HintAssist.SetHint(costToTxtB, Strings.CostUpTo);
            responsibleExpander.Header = Strings.Responsible;
            respEmployeeRBtn.Content = Strings.Employee;
            respNumberRBtn.Content = Strings.OfficeNumber;
            computerExpander.Header = Strings.Computer;
            compNumberRBtn.Content = Strings.Number;
            dateExpander.Header = Strings.Date;
            dateAcquisitionRBtn.Content = Strings.Acquisition;
            dateProductionRBtn.Content = Strings.Production;
            suppliersExpander.Header = Strings.Supplier;
            supNameRBtn.Content = Strings.Supplier;
            HintAssist.SetHint(templateQueriesCB, Strings.SelectRequest);
            absentTxtB.Text = Strings.Absent + ":";
            repairTxtB.Text = Strings.InRepair + ":";
            findBtn.Content = Strings.Find;
            cleanBtn.ToolTip = Strings.Clean;
        }

        #endregion

        private void Page_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

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

        private void RadioButtonChanged_Checked(object sender, RoutedEventArgs e) => CheckPriceContainer();

        private void ValidationPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^[0-9]*(\.[0-9]*)?$");

            var textBox = sender as TextBox;
            string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

            e.Handled = !regex.IsMatch(newText);
        }

        private void CleanTheForm_Click(object sender, RoutedEventArgs e) { } // Очистить форму

        private void TriggerANotification(string message)
        {
            if (NotificationSnackbar.MessageQueue is { } messageQueue && NotificationSnackbar.Message == null)
            {
                Task task = Task.Factory.StartNew(() => messageQueue.Enqueue(message));
            }
        }

        private void ExpendFilters(bool isExpended)
        {
            foreach(Expander expander in filterContainer.Children.OfType<Expander>())
            {
                expander.IsExpanded = isExpended;
            }
        }

        private void CheckPriceContainer()
        {
            bool isChecked = techCostRBtn.IsChecked.GetValueOrDefault();

            foreach (TextBox textBox in costFields.Children.OfType<TextBox>())
            {
                textBox.IsReadOnly = !isChecked;
                textBox.Text = isChecked ? textBox.Text : string.Empty;
            }
        }
    }
}
