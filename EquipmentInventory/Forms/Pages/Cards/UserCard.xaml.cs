using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для UserCard.xaml
/// </summary>
public partial class UserCard : UserControl
{
    private object _item;

    public UserCard(object item)
    {
        InitializeComponent();
        _item = item;
        if (item != null) MessageBox.Show(_item.ToString());
    }
}
