using EquipmentInventory.Classes.Data.Enums;
using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace EquipmentInventory.Forms.Pages.Admin;

/// <summary>
/// Логика взаимодействия для AdminPanel.xaml
/// </summary>
public partial class UserPanel : UserControl, IMainPanel
{
    private IMainWindow _parentWindow;
    private TabType _tabType;
    private DispatcherTimer closePopupTimer;
    private bool _isAdmin;

    public UserPanel(IMainWindow parentWindow, TabType tabType, bool isAdmin)
    {
        InitializeComponent();
        _parentWindow = parentWindow;
        _tabType = tabType;
        _isAdmin = isAdmin;
        InitializeParams();
        InitializeUI();
        InitializePopupTimer();
    }

    public UserPanel(IMainWindow parentWindow, bool isAdmin)
        :this(parentWindow, TabType.Default, isAdmin)
    {
    }

    #region Load
    private void InitializeParams()
    {
        var tabMapping = new Dictionary<TabType, object>
        {
            { TabType.Registration, userRegistrationRb },
            { TabType.Tables, tablesRb },
            { TabType.Dictionary, dictionariesRb }
        };

        if (tabMapping.TryGetValue(_tabType, out var tab))
        {
            SelectTheTab(tab);
        }
    }

    private void InitializeUI()
    {
        userRegistrationRb.Content = Strings.UserRegistration.ToUpper();
        tablesRb.Content = Strings.Tables.ToUpper();
        dictionariesRb.Content = Strings.Dictionaries.ToUpper();
        inventoryBtn.Content = Strings.Inventory;
        archiveBtn.Content = Strings.Archive;

        if (!_isAdmin)
        {
            userRegistrationRb.Visibility = Visibility.Collapsed;
            dictionariesRb.Visibility = Visibility.Collapsed;
        }
    }

    private void InitializePopupTimer()
    {
        closePopupTimer = new DispatcherTimer();
        closePopupTimer.Interval = TimeSpan.FromMilliseconds(350);
        closePopupTimer.Tick += (s, e) =>
        {
            closePopupTimer.Stop();
            actionsPopupRb.IsOpen = false;
        };
    }
    #endregion

    #region Control panel
    public void Archive_Click(object sender, RoutedEventArgs e)
        => SelectTable(TableType.Archive);

    public void Inventory_Click(object sender, RoutedEventArgs e)
        => SelectTable(TableType.Inventory);

    private void RadioButton_Click(object sender, RoutedEventArgs e) 
        => SelectTheTab(sender);

    private void SelectTheTab(object sender)
    {
        if (sender is RadioButton selectedRadioButton)
        {
            UpdateColor(selectedRadioButton);
            UpdatePage(selectedRadioButton);
        }
    }

    private void TablesRb_MouseEnter(object sender, MouseEventArgs e)
    {
        closePopupTimer.Stop();
        actionsPopupRb.IsOpen = true;
    }

    private void TablesRb_MouseLeave(object sender, MouseEventArgs e)
        => closePopupTimer.Start();

    private void PopupContent_MouseEnter(object sender, MouseEventArgs e)
        => closePopupTimer.Stop();

    private void PopupContent_MouseLeave(object sender, MouseEventArgs e)
        => closePopupTimer.Start();

    private void UpdatePage(RadioButton selectedRadioButton)
    {
        if (selectedRadioButton == null) return;

        var pageMappings = new Dictionary<string, Action>
        {
            { "userRegistrationRb", () => _parentWindow.ChangeMainFrameContent(new UserRegistration()) },
            { "tablesRb", () =>
                {
                    actionsPopupRb.IsOpen = false;
                    _parentWindow.ChangeMainFrameContent(new Tables(this));
                } 
            },
            { "dictionariesRb", () => _parentWindow.ChangeMainFrameContent(new Dictionaries()) }
        };

        if (pageMappings.TryGetValue(selectedRadioButton.Name, out var action))
        {
            action.Invoke();
        }
    }

    private void UpdateColor(RadioButton selectedRadioButton)
    {
        if (selectedRadioButton == null) return;

        SolidColorBrush secondaryColor = (SolidColorBrush)Application.Current.Resources["SecondaryColor"];

        SetRadioButtonDefault();

        selectedRadioButton.Foreground = secondaryColor;
        selectedRadioButton.IsChecked = true;
    }

    public void SetRadioButtonDefault()
    {
        SolidColorBrush textColor = (SolidColorBrush)Application.Current.Resources["TextColor"];

        foreach (var child in radioButtonContainer.Children.OfType<RadioButton>())
        {
            child.Foreground = textColor;
            child.IsChecked = false;
        }
    }

    private void SelectTable(TableType tab)
    {
        UpdateColor(tablesRb);
        _parentWindow.ChangeMainFrameContent(new TableSwitcher(tab));
        actionsPopupRb.IsOpen = false;
    }
    #endregion
}
    