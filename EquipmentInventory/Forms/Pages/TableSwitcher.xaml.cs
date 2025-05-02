using EquipmentInventory.Classes.Data.Enums;
using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Classes.Helpers;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Forms.Pages.GridTables;
using EquipmentInventory.Properties;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Pages;

/// <summary>
/// Логика взаимодействия для Inventory.xaml
/// </summary>
public partial class TableSwitcher : UserControl
{
    private TableType _tableType;

    public TableSwitcher(TableType tableType)
    {
        InitializeComponent();
        _tableType = tableType;
        InitializeUI();
        InitializeTable();
    }

    #region Load
    private void InitializeUI()
    {
        bool isCurrencyUsd = Settings.Default.CultureInfo == "en_US";

        HintAssist.SetHint(searchDatePc, Strings.Search);
        viewFiltersBtn.Content = Strings.View;
        hidenFiltersBtn.Content = Strings.Hide;
        techniqueExpander.Header = Strings.Technique;
        techNameRBtn.Content = Strings.Name;
        techTypeRBtn.Content = Strings.Type;
        techNumberRBtn.Content = Strings.InventoryNumber;
        techCostRBtn.Content = Strings.Price;
        HintAssist.SetHint(costFromTxtB, Strings.CostFrom);
        HintAssist.SetHint(costToTxtB, Strings.CostUpTo);
        responsibleExpander.Header = Strings.Responsible;
        respEmployeeRBtn.Content = Strings.Employee;
        respNumberRBtn.Content = Strings.OfficeNumber;
        computerExpander.Header = Strings.Computer;
        compNumberRBtn.Content = Strings.Number;
        dateExpander.Header = Strings.Date;
        dateAcquisitionRBtn.Content = Strings.Acquisition;
        dateProductionRBtn.Content = Strings.Production;
        dateOfUseRBtn.Content = Strings.DateOfUse;
        suppliersExpander.Header = Strings.Supplier;
        supNameRBtn.Content = Strings.Name;
        HintAssist.SetHint(templateQueriesCB, Strings.SelectRequest);
        absentTxtB.Text = Strings.Absent + ":";
        repairTxtB.Text = Strings.InRepair + ":";
        findBtn.Content = Strings.Find;
        cleanBtn.ToolTip = Strings.Clean;
        exportPopupBox.ToolTip = Strings.Export;

        TextFieldAssist.SetLeadingIcon(
            costFromTxtB, isCurrencyUsd 
                ? PackIconKind.CurrencyUsd 
                : PackIconKind.CurrencyRub);

        TextFieldAssist.SetLeadingIcon(
            costToTxtB, 
            isCurrencyUsd 
                ? PackIconKind.CurrencyUsd 
                : PackIconKind.CurrencyRub);
    }

    private void InitializeTable()
    {
        var tableMapping = new Dictionary<TableType, Action>
        {
            { TableType.Inventory, async () =>
                {
                    var viewModel = await TableSwitcherInventoryViewModel.CreateAsync(new NotificationService(notificationSnackbar));
                    DataContext = viewModel;
                    tableFrame.Content = new Inventory(viewModel);
                } 
            },
            { TableType.Archive, () =>
                {
                    var viewModel = new TablesSwitcherArchiveViewModel(new NotificationService(notificationSnackbar));
                    DataContext = viewModel;
                    tableFrame.Content = new Archive(viewModel);
                    CollapseFilters();
                }
            }
        };

        if (tableMapping.TryGetValue(_tableType, out var action))
        {
            action.Invoke();
        }
    }

    private void CollapseFilters()
    {
        filterButtonUnit.Visibility = Visibility.Hidden;
        filterUnit.Visibility = Visibility.Hidden;
        controlUnit.Visibility = Visibility.Hidden;
    }
    #endregion

    #region Search panel
    private void Page_MouseDown(object sender, MouseButtonEventArgs e) 
        => Focus();

    private void ToggleGridBtn_Click(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton toggle)
        {
            var command = toggle.IsChecked == true ? DrawerHost.OpenDrawerCommand : DrawerHost.CloseDrawerCommand;
            command.Execute(null, null);
        }
    }

    private void ExpendFilters_Click(Object sender, RoutedEventArgs e) 
        => ExpendFilters(bool.TryParse(((Button)sender).Tag as string, out bool isExpended));

    private void ValidationPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        var textBox = sender as TextBox;
        if (textBox == null) return;

        e.Handled = !ValidationHelper.IsValidPriceInput(textBox, e.Text);
    }

    private void ExpendFilters(bool isExpended)
    {
        foreach(Expander expander in filterContainer.Children.OfType<Expander>())
        {
            expander.IsExpanded = isExpended;
        }
    }
    #endregion
}
