using System.Windows.Controls;

namespace EquipmentInventory.Classes.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo(Page page);
        void ClearHistory();
    }
}
