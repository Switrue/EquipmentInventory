using EquipmentInventory.Classes.Interfaces;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Models.ViewModels
{
    public class WindowManagementViewModel
    {
        public IWindowState WindowState { get; }
        private IWindowService _windowService;

        public ICommand MaximizeCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand MinimizeCommand { get; }

        public WindowManagementViewModel(IWindowService windowService)
        {
            _windowService = windowService;

            CloseCommand = new RelayCommand(() => _windowService.CloseWindow());
            MinimizeCommand = new RelayCommand(() => _windowService.MinimizeWindow());
        }

        public WindowManagementViewModel(IWindowService windowService, IWindowState windowState) 
            : this(windowService)
        {
            WindowState = windowState;

            MaximizeCommand = new RelayCommand(() =>
            {
                _windowService.ToggleWindowState();
                WindowState.IsMaximized = _windowService.IsMaximized;
            });
        }
    }
}
