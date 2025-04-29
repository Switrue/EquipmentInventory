using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Classes.Services;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Admin;

/// <summary>
/// Логика взаимодействия для Dictionaries.xaml
/// </summary>
public partial class Dictionaries : UserControl
{
    private DictionariesViewModel viewModel;

    public Dictionaries()
    {
        InitializeComponent();
        var notification = new NotificationService(notificationSnackbar);
        viewModel = new DictionariesViewModel(notification);
        DataContext = viewModel;
    }

    private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        => Focus();
}
