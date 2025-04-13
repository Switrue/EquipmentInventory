using System.Windows.Controls;

namespace EquipmentInventory.Classes.Data.Interfaces;

public interface IMainWindow
{
    void ChangeMainFrameContent(UserControl newContent);
    void SetUsernameOnTheMainWindow();
    void SetUserImageOnTgeMainWindow();
}
