using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace TeamLeaderWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        public App () : base()
        {
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("ru-RU");
        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            TeamLeaderWpf.Properties.Settings.Default.isReminderAlreadyShown = false;
            TeamLeaderWpf.Properties.Settings.Default.Save();
        }
    }

    public class CultureAwareBinding : Binding
    {
        public CultureAwareBinding()
        {
            ConverterCulture = CultureInfo.CurrentCulture;
        }
    }


}
