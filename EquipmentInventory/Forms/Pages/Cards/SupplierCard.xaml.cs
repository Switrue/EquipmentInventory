using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для SupplierCard.xaml
/// </summary>
public partial class SupplierCard : UserControl
{
    private object _item;

    public SupplierCard(object item)
    {
        InitializeComponent();
        _item = item;
        if (_item != null) MessageBox.Show(_item.ToString());
    }
}
