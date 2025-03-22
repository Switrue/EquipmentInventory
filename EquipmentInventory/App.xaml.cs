using EquipmentInventory.Properties;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace EquipmentInventory
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            CultureInfo cultureInfo;

            try
            {
                cultureInfo = new CultureInfo(Settings.Default.CultureInfo);
            }
            catch 
            {
                cultureInfo = new CultureInfo("ru_RU");
            }
            
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            Thread.CurrentThread.CurrentUICulture = cultureInfo;
        }
    }
}
