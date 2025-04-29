using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для PositionCard.xaml
/// </summary>
public partial class PositionCard : UserControl
{
    private object _item;

    public PositionCard(object item)
    {
        InitializeComponent();
        _item = item;
        if (item != null) MessageBox.Show(_item.ToString());
    }
}
