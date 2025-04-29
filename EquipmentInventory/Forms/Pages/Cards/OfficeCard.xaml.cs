using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для OfficeCard.xaml
/// </summary>
public partial class OfficeCard : UserControl
{
    private object _item;

    public OfficeCard(object item)
    {
        InitializeComponent();
        _item = item;
        if (item != null ) MessageBox.Show(_item.ToString());
    }
}
