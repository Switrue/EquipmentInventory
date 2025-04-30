using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.ViewModels;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для TypeTechniqueCard.xaml
/// </summary>
public partial class TypeTechniqueCard : UserControl
{
    public TypeTechniqueCard(IDictionariesViewModel dictionaries, object selectedItem)
    {
        InitializeComponent();
        DataContext = new TypeTechniqueCardViewModel(dictionaries, selectedItem);
    }
}
