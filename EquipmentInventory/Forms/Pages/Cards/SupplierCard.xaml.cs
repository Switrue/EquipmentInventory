using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.ViewModels;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для SupplierCard.xaml
/// </summary>
public partial class SupplierCard : UserControl
{
    public SupplierCard(IDictionariesViewModel dictionariesView, object selectedItem)
    {
        InitializeComponent();
        DataContext = new SupplierCardViewModel(dictionariesView, selectedItem);
    }
}
