using System.Windows.Controls;

namespace EquipmentInventory.Classes.Data.Interfaces;

public interface IProgramInformationService
{
    void SetTitle(string header);
    void SetDescription(string description);
}
