using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для ComputerCard.xaml
/// </summary>
public partial class ComputerCard : UserControl
{
    private object _item;

    public ComputerCard(object item)
    {
        InitializeComponent();
        _item = item;
        if (item != null) MessageBox.Show(_item.ToString());
    }
}
