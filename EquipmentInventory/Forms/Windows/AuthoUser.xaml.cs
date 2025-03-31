using EquipmentInventory.Properties;
using System;
using System.Windows;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Npgsql;
using EquipmentInventory.Classes.Cryptography;
using EquipmentInventory.Classes.Helper;
using EquipmentInventory.Classes.Data;
using EquipmentInventory.Classes.Data.Database;
using EquipmentInventory.Classes.Helpers;
using System.Windows.Controls;
using Validation = EquipmentInventory.Classes.Data.Validation;
using EquipmentInventory.Classes.Services;

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
            windowTitle.Text = Strings.AuthoTitle;
            Title = windowTitle.Text;
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
            TextFieldHelper.ClearAllTextFields(textFieldContainer);
        }

        #endregion

        #region Authorization

        private void Login_Click(object sender, EventArgs e)
        {
            if (Validation.AnyTextBoxIsEmpty(textFieldContainer))
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
                "select id from users where id = (select user_id from tokens t where access_token = @token)", 
                new NpgsqlParameter("@token", token)
            ).Rows[0];

            // Данные пользователя
            var user = new UserData(Convert.ToInt64(authResult[0]));
            var userService = new AuthorizationService(user);

            userService.InitializeMainWindow();

            Close();
        }

        #endregion

        #region Valid changed

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e) => Validation.IsTextBoxEmpty((TextBox)sender);

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e) => Validation.IsPasswordBoxEmpty((PasswordBox)sender);

        #endregion
    }
}
