using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Classes.Helpers;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Pages.GridTables;

/// <summary>
/// Логика взаимодействия для Inventory.xaml
/// </summary>
public partial class Inventory : UserControl
{
    public Inventory(TableSwitcherInventoryViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void ValidationPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null) return;

        e.Handled = !ValidationHelper.IsValidPriceInput(textBox, e.Text);
    }

    private void DatePicker_PreviewTextInput(object sender, TextCompositionEventArgs e)
        => e.Handled = !Regex.IsMatch(e.Text, @"[\d.]");
}
