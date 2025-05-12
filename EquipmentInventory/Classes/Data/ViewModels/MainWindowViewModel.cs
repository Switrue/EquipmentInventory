using EquipmentInventory.Classes.Data.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace EquipmentInventory.Classes.Data.ViewModels;

public class MainWindowViewModel : INotifyPropertyChanged
{
    public WindowManagementViewModel WindowManagement { get; }
    public bool IsLoad => App.Loading;

    public MainWindowViewModel(IWindowService windowService, IWindowState windowState)
    {
        App.OnLoadingChanged += (s, e) => OnPropertyChanged(nameof(IsLoad));
        WindowManagement = new WindowManagementViewModel(windowService, windowState);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
