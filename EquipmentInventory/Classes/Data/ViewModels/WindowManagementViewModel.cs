using EquipmentInventory.Classes.Data.Interfaces;
using GalaSoft.MvvmLight.Command;
using System.Windows.Input;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class WindowManagementViewModel
{
    private IWindowService _windowService;

    public IWindowState WindowState { get; }
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
