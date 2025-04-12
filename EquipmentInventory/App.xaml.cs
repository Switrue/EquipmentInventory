using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Forms.Windows;
using EquipmentInventory.Properties;
using System;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Windows;

namespace EquipmentInventory
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static CultureInfo cultureInfo;

        public static HttpClient ApiClient { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeApiClient();
            InitializeCulture();
            InitializeMainWindow();
        }

        #region Load

        private async void InitializeMainWindow()
        {
            var jwt = Settings.Default.UserToken;

            if (!string.IsNullOrWhiteSpace(jwt))
            {
                if (await AuthoRequest.CheckAuthorization())
                {
                    await AuthorizationService.Authorize(jwt);
                    return;
                }
            }

            new AuthoUser().Show();
        }

        private void InitializeCulture()
        {
            try
            {
                cultureInfo = new CultureInfo(Settings.Default.CultureInfo);
            }
            catch
            {
                cultureInfo = new CultureInfo("ru-RU");
            }

            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }

        private void InitializeApiClient()
        {
            ApiClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7278")
            };

            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            if (!string.IsNullOrEmpty(Settings.Default.UserToken))
            {
                ApiClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", Settings.Default.UserToken);
            }
        }

        #endregion

        public static void SetAuthorizationToken(string token)
        {
            if (ApiClient != null)
            {
                ApiClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }
    }
}
