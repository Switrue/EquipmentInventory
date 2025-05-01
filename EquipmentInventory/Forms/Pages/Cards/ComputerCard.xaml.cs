using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Classes.Helpers;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для ComputerCard.xaml
/// </summary>
public partial class ComputerCard : UserControl
{
    public ComputerCard(IDictionariesViewModel viewModel, object selectedItem)
    {
        InitializeComponent();
        DataContext = new ComputerCardViewModel(viewModel, selectedItem);
    }

    private void ValidationNumber_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        => e.Handled = !ValidationHelper.IsValidNumber(e.Text);
}
