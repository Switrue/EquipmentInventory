using EquipmentInventory.Classes.Data.Models;
using EquipmentInventory.Classes.Data.Requests;
using EquipmentInventory.Classes.Services;
using EquipmentInventory.Forms.Windows;
using EquipmentInventory.Properties;
using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.IO;
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
        private static Mutex _mutex;
        private const string MutexName = "EquipmentInventory.Mutex";
        private static IConfiguration configuration;
        private static CultureInfo cultureInfo;

        private static bool _loading;
        public static bool Loading
        {
            get => _loading;
            set
            {
                if (_loading != value)
                {
                    _loading = value;
                    OnLoadingChanged?.Invoke(null, EventArgs.Empty);
                }
            }
        }

        public static HttpClient ApiClient { get; private set; }
        public static Users user { get; private set; }

        public static event EventHandler OnLoadingChanged;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            if (!IsSingleInstance())
            {
                Shutdown();
                return;
            }

            InitializeServices();
            InitializeApiClient();
            InitializeCulture();
            InitializeMainWindow();
        }

        #region Load
        private bool IsSingleInstance()
        {
            _mutex = new Mutex(true, MutexName, out bool isNewInstance);
            return isNewInstance;
        }

        private void InitializeServices()
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

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
            try
            {
                var apiSettings = configuration.GetSection("ApiSettings").Get<ApiSettings>();

                ApiClient = new HttpClient
                {
                    BaseAddress = new Uri(apiSettings.BaseUrl)
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
            catch (Exception ex)
            {
                MessageBox.Show($"{Strings.Error}: {ex.Message}", Strings.Error, MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }
        #endregion

        #region Methods
        public static void SetAuthorizationToken(string token)
        {
            if (ApiClient != null)
            {
                ApiClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public static void SetUser(Users newUser)
            => user = newUser;

        public static void SetUserImage(byte[] bytes)
        {
            try
            {
                user.Image = bytes;
            }
            catch
            {
                throw new Exception("user image error");
            }
        }
        #endregion
    }
}
