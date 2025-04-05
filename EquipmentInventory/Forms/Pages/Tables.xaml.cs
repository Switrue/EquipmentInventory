using EquipmentInventory.Classes.Interfaces;
using EquipmentInventory.Classes.Models;
using EquipmentInventory.Properties;
using System.Windows;
using System.Windows.Controls;

namespace EquipmentInventory.Forms.Pages;

/// <summary>
/// Логика взаимодействия для Tables.xaml
/// </summary>
public partial class Tables : UserControl
{
    private IMainPanel _parent;

    public Tables(IMainPanel parent)
    {
        InitializeComponent();
        _parent = parent;
        InitializeUI();
        InitializeParams();
    }

    #region Load

    private void InitializeUI()
    {
        titleArchiveCardTxtBl.Text = Strings.Archive;
        descriptionArchiveCardTxtBl.Text = Strings.ArchiveDescripton;
        titleInventoryCardTxtBl.Text = Strings.Inventory;
        descriptionInventoryCardTxtBl.Text = Strings.InventoryDescription;
        titleBlockTxtBl.Text = Strings.Tables;
    }

    private void InitializeParams()
    {
        var iconData = new IconData
        {
            Kind = "NavigateNext",
            Size = 25
        };
        DataContext = iconData;
    }

    #endregion

    #region Click

    private void GoToArchive_Click(object sender, RoutedEventArgs e) => _parent.Archive_Click(sender, e);

    private void GoToInventory_Click(object sender, RoutedEventArgs e) => _parent.Inventory_Click(sender, e);

    #endregion
}
