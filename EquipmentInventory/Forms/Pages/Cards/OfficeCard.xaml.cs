using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Classes.Helpers;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для OfficeCard.xaml
/// </summary>
public partial class OfficeCard : UserControl
{
    public OfficeCard(IDictionariesViewModel dictionariesView, object selectedItem)
    {
        InitializeComponent();
        DataContext = new OfficeCardViewModel(dictionariesView, selectedItem);
    }

    private void ValidationNumber_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        => e.Handled = !ValidationHelper.IsValidNumber(e.Text);
}
