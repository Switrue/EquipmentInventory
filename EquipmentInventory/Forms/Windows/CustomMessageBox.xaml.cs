using EquipmentInventory.Properties;
using System;
using System.Windows;
using System.Windows.Input;

namespace EquipmentInventory.Forms.Windows
{
    /// <summary>
    /// Логика взаимодействия для CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        private string _title;

        private string _message;

        private bool _visibility;

        public CustomMessageBox(string title, string message, bool visibility)
        {
            InitializeComponent();

            // Получение параметров
            _title = title;
            _message = message;
            _visibility = visibility;

            InitializeUI();
            DefaultParams();
        }

        #region Load

        private void InitializeUI()
        {
            Title = _title;
            closeBtn.ToolTip = Strings.Close;
            trueBtn.Content = _visibility ? Strings.Ok : Strings.Yes;
            falseBtn.Content = Strings.No;
        }

        private void DefaultParams()
        {
            customMessageBoxTitle.Text = _title;
            messageTxtB.Text = _message;
            falseBtn.Visibility = !_visibility ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        #region Window management

        private void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseWindow_Click(Object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void ReturnTrue_Click(object sender, EventArgs e)
        {
            DialogResult = true;
        }

        private void ReturnFalse_Click(object sender, EventArgs e)
        {
            DialogResult = false;
        }
    }
}
