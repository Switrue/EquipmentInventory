namespace EquipmentInventory.Classes.Interfaces
{
    public interface IWindowService
    {
        void CloseWindow();

        void MinimizeWindow();

        void ToggleWindowState();

        bool IsMaximized { get; }
    }
}
