using EquipmentInventory.Classes.Interfaces;
using EquipmentInventory.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EquipmentInventory.Forms.Pages.Admin
{
    /// <summary>
    /// Логика взаимодействия для AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Page, IMainPanel
    {
        private IMainWindow _parentWindow;
        private UserRegistration userRegistration = new UserRegistration();
        private Dictionaries dictionaries = new Dictionaries();
        private Tables tables;

        public AdminPanel(IMainWindow parentWindow)
        {
            InitializeComponent();
            _parentWindow = parentWindow;
            InitializeData();
            InitializeUI();
        }

        #region Load

        private void InitializeData()
        {
            tables = new Tables(this);
            RadioButton_Click(tablesRb, null);
        }

        private void InitializeUI()
        {
            userRegistrationRb.Content = Strings.UserRegistration.ToUpper();
            tablesRb.Content = Strings.Tables.ToUpper();
            dictionariesRb.Content = Strings.Dictionaries.ToUpper();
            inventoryBtn.Content = Strings.Inventory;
            archiveBtn.Content = Strings.Archive;
        }

        #endregion

        #region Control panel

        private void DisplayTableOptions_Mouse(object sender, MouseEventArgs e) => actionsPopupRb.IsOpen = !actionsPopupRb.IsOpen;

        public void Archive_Click(object sender, RoutedEventArgs e) => MessageBox.Show("Pressed Archive from Adm");

        public void Inventory_Click(object sender, RoutedEventArgs e) => _parentWindow.ChangeMainFrameContent(new Inventory());

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton selectedRadioButton)
            {
                UpdateColor(selectedRadioButton);
                UpdatePage(selectedRadioButton);
            }
        }

        private void UpdatePage(RadioButton selectedRadioButton)
        {
            if (selectedRadioButton == null) return;

            var pageMappings = new Dictionary<string, Action>
            {
                { "userRegistrationRb", () => _parentWindow.ChangeMainFrameContent(userRegistration) },
                { "tablesRb", () =>
                    {
                        actionsPopupRb.IsOpen = false;
                        _parentWindow.ChangeMainFrameContent(tables);
                    } 
                },
                { "dictionariesRb", () => _parentWindow.ChangeMainFrameContent(dictionaries) }
            };

            if (pageMappings.TryGetValue(selectedRadioButton.Name, out var action))
            {
                action.Invoke();
            }
        }

        private void UpdateColor(RadioButton selectedRadioButton)
        {
            if (selectedRadioButton == null) return;

            SolidColorBrush textColor = (SolidColorBrush)Application.Current.Resources["TextColor"];
            SolidColorBrush secondaryColor = (SolidColorBrush)Application.Current.Resources["SecondaryColor"];

            SetRadioButtonColors(textColor);

            selectedRadioButton.Foreground = secondaryColor;
        }

        private void SetRadioButtonColors(SolidColorBrush textColor)
        {
            foreach (var child in radioButtonContainer.Children.OfType<RadioButton>())
            {
                child.Foreground = textColor;
            }
        }

        #endregion
    }
}
