using EquipmentInventory.Classes.Data.Interfaces;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace EquipmentInventory.Classes.Handlers;

public class WindowStateHandler : IWindowState
{
    private bool _isMaximized;

    public event PropertyChangedEventHandler? PropertyChanged;
    public double PreviousHeight { get; set; }
    public double PreviousWidth { get; set; }
    public bool IsMaximized
    {
        get => _isMaximized;
        set
        {
            _isMaximized = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(MaximizeButtonContent));
            OnPropertyChanged(nameof(ResizeMarkerVisibility));
            OnPropertyChanged(nameof(WindowCornerRadius));
            OnPropertyChanged(nameof(FooterCornerRadius));
        }
    }
    public string MaximizeButtonContent => 
        IsMaximized ? "WindowRestore" : "WindowMaximize";
    public Visibility ResizeMarkerVisibility => 
        IsMaximized ? Visibility.Collapsed : Visibility.Visible;
    public CornerRadius WindowCornerRadius => 
        IsMaximized ? new CornerRadius(0) : new CornerRadius(10);
    public CornerRadius FooterCornerRadius => 
        IsMaximized ? new CornerRadius(0) : new CornerRadius(0, 0, 10, 10);

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
