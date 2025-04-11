using EquipmentInventory.Classes.Data.Requests;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages.Admin;

/// <summary>
/// Логика взаимодействия для Dictionaries.xaml
/// </summary>
public partial class Dictionaries : UserControl
{
    public Dictionaries()
    {
        InitializeComponent();
    }

    private async void Button_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        var user = await UserRequest.GetUser(150);
        MessageBox.Show(user?.Username);
    }
}
