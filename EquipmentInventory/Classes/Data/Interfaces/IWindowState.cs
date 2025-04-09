using System.ComponentModel;
using System.Windows;

namespace EquipmentInventory.Classes.Data.Interfaces;

public interface IWindowState : INotifyPropertyChanged
{
    bool IsMaximized { get; set; }
    double PreviousHeight { get; set; }
    double PreviousWidth { get; set; }
    string MaximizeButtonContent { get; }
    Visibility ResizeMarkerVisibility { get; }
    CornerRadius WindowCornerRadius { get; }
    CornerRadius FooterCornerRadius { get; }
}
