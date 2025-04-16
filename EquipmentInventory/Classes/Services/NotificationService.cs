using MaterialDesignThemes.Wpf;
using System.Threading.Tasks;

namespace EquipmentInventory.Classes.Services;

class NotificationService
{
    private Snackbar _notification;

    public NotificationService(Snackbar notification) 
        => _notification = notification;

    public void Show(string message)
    {
        if (_notification.MessageQueue is { } messageQueue && _notification.Message == null)
        {
            Task task = Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }
    }
}
