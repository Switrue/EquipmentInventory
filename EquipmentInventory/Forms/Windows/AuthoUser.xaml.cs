using EquipmentInventory.Properties;
using System;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Npgsql;
using EquipmentInventory.Classes.Cryptography;
using EquipmentInventory.Classes.Helper;
using System.Windows.Media;
using System.Windows.Controls;
using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Data.Database;
using EquipmentInventory.Forms.Pages.Admin;

namespace EquipmentInventory.Forms.Windows
{
    /// <summary>
    /// Логика взаимодействия для AuthoUser.xaml
    /// </summary>
    public partial class AuthoUser : Window
    {
        public AuthoUser()
        {
            InitializeComponent();
            InitializeUI();
        }

        #region Load

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            try
            {
                var tokenResult = ConnectionDatabase.ExecuteQuery(
                    "select refresh_access_token(@token)", 
                    new NpgsqlParameter("@token", Settings.Default.UserToken)
                );
                var token = tokenResult.Rows[0][0].ToString();

                Settings.Default.UserToken = token;
                Settings.Default.Save();

                if (TokenValidation.IsTokenInvalid(Settings.Default.UserToken))
                {
                    return;
                }

                Authorization(token);
            }
            catch { }
        }

        private void InitializeUI()
        {
            authoTitle.Text = Strings.AuthoTitle;
            Title = Strings.AuthoTitle;
            loginBtn.Content = Strings.Login;
            collapseBtn.ToolTip = Strings.Collapse;
            closeBtn.ToolTip = Strings.Close;
            rememberUserChB.Content = Strings.RememberUser;
            HintAssist.SetHint(usernameTextB, Strings.Username);
            HintAssist.SetHint(passwordPsB, Strings.Password);
        }

        #endregion

        #region Window management

        private void DragWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => DragMove();

        private void CollapseWindow_Click(object sender, EventArgs e) => WindowState = WindowState.Minimized;

        private void CloseWindow_Click(object sender, EventArgs e) => Close();

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.ClearFocus();
            ClearTheFields();
        }

        #endregion

        #region Authorization

        private void Login_Click(object sender, EventArgs e)
        {
            ClearTheFields();

            if (ValidateTextBox(usernameTextB) || ValidatePasswordBox(passwordPsB))
            {
                return;
            }

            try
            {
                var authSql = "select * from authenticate_user(@log, @pass)";

                var authParams = new[]
                {
                    new NpgsqlParameter("@log", usernameTextB.Text),
                    new NpgsqlParameter("@pass", MD5Encryprion.GetMD5Hash(passwordPsB.Password))
                };

                var authResut = ConnectionDatabase.ExecuteQuery(authSql, authParams);
                var token = authResut.Rows[0][0].ToString();

                Settings.Default.UserToken = rememberUserChB.IsChecked == true ? token : string.Empty;
                Settings.Default.Save();

                Authorization(token);
            }
            catch (PostgresException ex)
            {
                switch (ex.ErrorCode)
                {
                    case -2147467259:
                        CustomMessageBoxHelper.Show(Strings.Error, Strings.AuthenticationError, false);
                        break;
                    default:
                        CustomMessageBoxHelper.Show(Strings.Error, ex.ErrorCode.ToString(), false);
                        break;
                }
            }
            catch
            {
                CustomMessageBoxHelper.Show(Strings.Error, Strings.DatabaseError, false);
            }
        }

        private void Authorization(string token)
        {
            var authResult = ConnectionDatabase.ExecuteQuery(
                "select id from users where id = (select user_id from tokens where access_token = @token)", 
                new NpgsqlParameter("@token", token)
            ).Rows[0];

            // Данные пользователя
            UserData user = new UserData(Convert.ToInt64(authResult[0]));

            MainWindow mainWindow = new MainWindow(user);
            var roleId = user.UserRoleId;

            if (roleId != 0)
            {
                switch (roleId)
                {
                    case 1:
                        mainWindow.ChangeControlPanelFrameContent(new UserPanel(mainWindow, true));
                        break;
                    case 2:
                        mainWindow.ChangeControlPanelFrameContent(new UserPanel(mainWindow, false));
                        break;
                    default:
                        CustomMessageBoxHelper.Show(Strings.Error, Strings.RoleNotFound, false);
                        break;
                }

                mainWindow.Show();
                Close();
            }
            else
            {
                // Данные пользователя не были получены 
            }
        }

        #endregion

        #region Validation

        private bool ValidateTextBox(TextBox textBox)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                SetTextBoxHelperText(textBox, Strings.FieldEmpty);
                return true;
            }
            return false;
        }

        private bool ValidatePasswordBox(PasswordBox passwordBox)
        {
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                SetTextBoxHelperText(passwordBox, Strings.FieldEmpty);
                return true;
            }
            return false;
        }

        private void ClearTheFields()
        {
            SetTextBoxHelperText(usernameTextB);
            SetTextBoxHelperText(passwordPsB);
        }

        private void SetTextBoxHelperText(Control element, string message, SolidColorBrush color)
        {
            SolidColorBrush textColor = (SolidColorBrush)Application.Current.Resources["TextColor"];
            element.Foreground = message == string.Empty ? textColor : color;
            HintAssist.SetForeground(element, color);
            TextFieldAssist.SetUnderlineBrush(element, color);
            HintAssist.SetHelperText(element, message);
        }

        private void SetTextBoxHelperText(Control element, string message)
        {
            SolidColorBrush accentColor = (SolidColorBrush)Application.Current.Resources["AccentColor"];
            SetTextBoxHelperText(element, message, accentColor);
        }

        private void SetTextBoxHelperText(Control element)
        {
            SolidColorBrush blueGreyColor = (SolidColorBrush)Application.Current.Resources["PrimaryColor"];
            SetTextBoxHelperText(element, string.Empty, blueGreyColor);
        }

        #endregion
    }
}
