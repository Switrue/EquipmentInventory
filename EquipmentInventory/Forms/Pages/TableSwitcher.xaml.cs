using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using EquipmentInventory.Properties;
using System.Windows.Media;
using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.Enums;
using EquipmentInventory.Forms.Pages.GridTables;
using EquipmentInventory.Classes.Services;
using System.Collections.Generic;

namespace EquipmentInventory.Forms.Pages;

/// <summary>
/// Логика взаимодействия для Inventory.xaml
/// </summary>
public partial class TableSwitcher : UserControl, IMainTableSwitcher
{
    private TableType _tableType;
    private NotificationService notification;

    public TableSwitcher(TableType tableType)
    {
        InitializeComponent();
        _tableType = tableType;
        InitializeUI();
        InitializeTable();
        InitializeParams();
    }

    #region Load
    private void InitializeUI()
    {
        bool isCurrencyUsd = Settings.Default.CultureInfo == "en_US";

        HintAssist.SetHint(searchTxtB, Strings.Search);
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
            { TableType.Inventory, () => tableFrame.Content = new Inventory(this) },
            { TableType.Archive, () =>
                {
                    tableFrame.Content = new Archive(this);
                    CollapseFilters();
                }
            }
        };

        if (tableMapping.TryGetValue(_tableType, out var action))
        {
            action.Invoke();
        }
    }

    private void InitializeParams()
    {
        notification = new NotificationService(notificationSnackbar);
    }

    private void CollapseFilters()
    {
        filterButtonUnit.Visibility = Visibility.Collapsed;
        filterUnit.Visibility = Visibility.Collapsed;
        templateQueriesCB.Visibility = Visibility.Collapsed;
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

    private void RadioButtonChanged_Checked(object sender, RoutedEventArgs e) 
        => CheckPriceContainer();

    private void ValidationPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
    {
        Regex regex = new Regex(@"^[0-9]*(\.[0-9]*)?$");

        var textBox = sender as TextBox;
        string newText = textBox.Text.Insert(textBox.CaretIndex, e.Text);

        e.Handled = !regex.IsMatch(newText);
    }

    private void CleanTheForm_Click(object sender, RoutedEventArgs e)
    {
        ResetExpanders();
        ClearSearchTextBox();
        ResetTemplateQueriesComboBox();
        SetDefaultListBoxSelections();
    }

    private void ClearSearchTextBox()
        => searchTxtB.Text = string.Empty;

    private void ResetTemplateQueriesComboBox()
        => templateQueriesCB.SelectedItem = null;

    private void SetDefaultListBoxSelections()
    {
        absentListB.SelectedIndex = 1;
        repairListB.SelectedIndex = 1;
    }

    private void ResetExpanders()
    {
        foreach (Expander expander in filterContainer.Children.OfType<Expander>())
        {
            ClearRadioButtons(expander);
        }

        // Очистить текстовые поля со стоимостью
        CheckPriceContainer();
    }

    private void ClearRadioButtons(DependencyObject parent)
    {
        if (parent == null) return;

        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is RadioButton radioButton)
            {
                radioButton.IsChecked = false;
            }
            else
            {
                ClearRadioButtons(child);
            }
        }
    }

    private void ExpendFilters(bool isExpended)
    {
        foreach(Expander expander in filterContainer.Children.OfType<Expander>())
        {
            expander.IsExpanded = isExpended;
        }
    }

    private void CheckPriceContainer()
    {
        bool isChecked = techCostRBtn.IsChecked.GetValueOrDefault();

        foreach (TextBox textBox in costFields.Children.OfType<TextBox>())
        {
            textBox.IsReadOnly = !isChecked;
            textBox.Text = isChecked ? textBox.Text : string.Empty;
        }
    }
    #endregion

    #region Interface methods
    public void TriggerANotification(string message)
        => notification.Show(message);
    #endregion
}
