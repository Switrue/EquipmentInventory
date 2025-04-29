using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для TypeTechniqueCard.xaml
/// </summary>
public partial class TypeTechniqueCard : UserControl
{
    private object _items;

    public TypeTechniqueCard(object items)
    {
        InitializeComponent();
        _items = items;
        if (_items != null) MessageBox.Show(_items.ToString());
    }
}
