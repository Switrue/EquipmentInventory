using EquipmentInventory.Classes.Data.ViewModels;
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
        DataContext = new DictionariesViewModel();
    }

    private void UserControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        => Focus();
}
