using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.ViewModels;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для PositionCard.xaml
/// </summary>
public partial class PositionCard : UserControl
{
    public PositionCard(IDictionariesViewModel viewModel, object selectedItem)
    {
        InitializeComponent();
        DataContext = new PositionCardViewModel(viewModel, selectedItem);
    }
}
