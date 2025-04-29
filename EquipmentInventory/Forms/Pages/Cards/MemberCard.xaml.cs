using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для MemberCard.xaml
/// </summary>
public partial class MemberCard : UserControl
{
    private object _item;

    public MemberCard(object item)
    {
        InitializeComponent();
        _item = item;
        if (_item != null) MessageBox.Show(_item.ToString());
    }
}
