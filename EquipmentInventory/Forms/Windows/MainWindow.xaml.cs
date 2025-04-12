using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Interfaces;
using EquipmentInventory.Classes.Data.ViewModels;
using EquipmentInventory.Classes.Handlers;
using EquipmentInventory.Forms.Pages.Admin;
using EquipmentInventory.Forms.Pages;
using EquipmentInventory.Properties;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using UserControl = System.Windows.Controls.UserControl;

namespace EquipmentInventory.Forms.Windows;

/// <summary>
/// Логика взаимодействия для MainWindow.xaml
/// </summary>
public partial class MainWindow : Window, IMainWindow
{
    private WindowStateHandler _windowState;
    private WindowService _windowService;
    private Users _user;
    private UserPanel _userPanel;
    private bool isResizing;

    public MainWindow(Users user)
    {
        InitializeComponent();
        _user = user;
        InitializeUI();
        InitializeParams();
    }

    #region Virtual methods

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);

        if (!_windowState.IsMaximized)
        {
            Settings.Default.WindowWidth = Width;
            Settings.Default.WindowHeight = Height;
            Settings.Default.Save();
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        base.OnKeyDown(e);

        if (e.Key == Key.F1 && Keyboard.Modifiers == ModifierKeys.Control)
        {
            AboutTheProgramm_Click(this, null);
        }
    }

    #endregion

    #region Load

    private void InitializeUI()
    {
        string username =  _user.Surname + " " + _user.Username;

        Title = Strings.MainWindowTitle;
        collapseBtn.ToolTip = Strings.Collapse;
        closeBtn.ToolTip = Strings.Close;
        maximizeBtn.ToolTip = Strings.Maximize;
        logOutBtn.Content = Strings.LogOut;
        userNameTexB.Text = string.IsNullOrWhiteSpace(username) ? Strings.DefaultUserName : username;
        profileBtn.Content = Strings.Profile;
        manageAccountBtn.ToolTip = Strings.ManageAccount;
        fileMnIt.Header = $"_{Strings.MainWindowTitle}";
        settingsMnIt.Header = $"_{Strings.Settings}";
        helpMnIt.Header = $"_{Strings.Help}";
        aboutTheProgrammMnIt.Header = $"_{Strings.AboutTheProgramm}";

        UserAccountService.SetImageSource(_user.Image, userImage);

        Width = Settings.Default.WindowWidth;
        Height = Settings.Default.WindowHeight;
    }

    private void InitializeParams()
    {
        _windowState = new WindowStateHandler();
        _windowService = new WindowService(this, _windowState);
        DataContext = new WindowManagementViewModel(_windowService, _windowState);
    }

    public void SaveUserPanelObject(UserPanel userPanel) => _userPanel = userPanel;

    #endregion

    #region Window management

    private async void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ClickCount == 2)
        {
            await Task.Delay(200);
            _windowService.ToggleWindowState();
        }
        else if (!_windowState.IsMaximized)
        {
            DragMove();
        }
    }

    private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        if (WindowState == WindowState.Maximized)
        {
            _windowService.ToggleWindowState();
            WindowState = WindowState.Normal;
        }
    }

    #endregion

    #region Resizing a window

    private void ResizeHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        isResizing = true;
        Mouse.Capture(resizeMarker);
    }

    private void ResizeHandle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        isResizing = false;
        Mouse.Capture(null);
    }

    private void ResizeHandle_MouseMove(object sender, MouseEventArgs e)
    {
        if (isResizing)
        {
            var mousePos = e.GetPosition(this);

            // Устанавливаем новые размеры, но не меньше минимальных значений
            double newWidth = mousePos.X + 10;
            double newHeight = mousePos.Y + 10;

            // Проверяем, чтобы новые размеры не были меньше минимальных
            if (newWidth >= MinWidth)
            {
                Width = newWidth;
            }

            if (newHeight >= MinHeight)
            {
                Height = newHeight;
            }
        }
    }

    #endregion

    #region Click

    private void Logout_Click(object sender, EventArgs e)
    {
        bool result = CustomMessageBoxHelper.Show(Strings.SignOut, Strings.SignOutDescription, true);

        if (result)
        {
            WindowService.ResetSettings();
            WindowService.RestoreApp();
        }
    }

    private void ShowProfileOptions_Click(object sender, RoutedEventArgs e) => ToggleActionsPopup();

    private void Settings_Click(object sender, RoutedEventArgs e) => WindowService.ShowDialogWindow(new ProgramSettings());

    private void AboutTheProgramm_Click(object sender, RoutedEventArgs e) => UpdatePageWithDefaultSettings(new ProgramInformation());

    private void Profile_Click(object sender, EventArgs e)
    {
        ToggleActionsPopup();
        UpdatePageWithDefaultSettings(new UserProfile(_user));
    } 

    private void ToggleActionsPopup()
    {
        actionsPopup.IsOpen = !actionsPopup.IsOpen;
    }

    #endregion

    #region Change frame

    public void ChangeControlPanelFrameContent(UserControl newContent)
    {
        _userPanel = (UserPanel)newContent;
        ChangeFrameContent(controlPanelFrame, newContent);
    }

    public void ChangeMainFrameContent(UserControl newContent) => ChangeFrameContent(mainFrame, newContent);

    private void UpdatePageWithDefaultSettings(UserControl newContent)
    {
        _userPanel.SetRadioButtonDefault();
        ChangeMainFrameContent(newContent);
    }

    private void ChangeFrameContent(ContentControl frame, UserControl newContent)
    {
        frame.Content = newContent;
    }

    #endregion
}
