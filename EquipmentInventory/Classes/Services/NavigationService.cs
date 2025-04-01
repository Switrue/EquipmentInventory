using EquipmentInventory.Classes.Interfaces;
using System.Windows.Controls;

namespace EquipmentInventory.Classes.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;

        public NavigationService() { }

        public void RegisterFrame(Frame frame) => _frame = frame;

        public void NavigateTo(Page page)
        {
            _frame?.Navigate(page);
        }

        public void ClearHistory()
        {
            if (_frame != null && _frame.CanGoBack)
            {
                _frame.RemoveBackEntry();
            }
        }
    }
}
