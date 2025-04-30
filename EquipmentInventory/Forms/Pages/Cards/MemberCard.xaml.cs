using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.ViewModels;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Cards;

/// <summary>
/// Логика взаимодействия для MemberCard.xaml
/// </summary>
public partial class MemberCard : UserControl
{
    private readonly IDictionariesViewModel _dictionariesView;
    private readonly object _selectedItem;

    public MemberCard(IDictionariesViewModel dictionariesView, object selectedItem)
    {
        InitializeComponent();
        _dictionariesView = dictionariesView;
        _selectedItem = selectedItem;
        InitializeDataContext();
    }

    private async void InitializeDataContext()
    {
        var view = await MemberCardViewModel.CreateAsync(_dictionariesView, _selectedItem);
        DataContext = view;
    } 
}
