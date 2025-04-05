using EquipmentInventory.Classes.Interfaces;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Services;

public class NavigationService : INavigationService
{
    private ContentControl _frame;

    public NavigationService() { }

    public void RegisterFrame(ContentControl frame) => _frame = frame;

    public void NavigateTo(UserControl page)
    {
        _frame.Content = page;
    }
}
