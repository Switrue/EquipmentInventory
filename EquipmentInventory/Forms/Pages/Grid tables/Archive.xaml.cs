using EquipmentInventory.Classes.Data.ViewModels;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.GridTables;

/// <summary>
/// Логика взаимодействия для Archive.xaml
/// </summary>
public partial class Archive : UserControl
{
    public Archive(TablesSwitcherArchiveViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
