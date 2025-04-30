using System.ComponentModel;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Data.Interfaces;

public interface IDictionariesViewModel : INotifyPropertyChanged
{
    void TriggerANotification(string message);
    Task UpdateItemsAsync();
}
