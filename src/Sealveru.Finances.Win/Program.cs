using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Adapters;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using DevExpress.XtraEditors;

namespace Sealveru.Finances.Win
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            ApplyXafConfigurations();
          
            var app = new FinancesWindowsFormsApplication {EnableModelCache = true};
            var security = (SecurityStrategy)app.Security;
            security.RegisterXPOAdapterProviders();
            SecurityAdapterHelper.Enable(ReloadPermissionStrategy.CacheOnFirstAccess);

            SetupConnectionString(app);
            SetupDatabaseUpdateMode(app);

            try
            {
                app.Setup();
                app.Start();
            }
            catch (Exception e)
            {
                app.HandleException(e);
            }
        }


        private static void SetupConnectionString(XafApplication app)
        {
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) 
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
#else
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null)
                app.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
#endif
        }
        private static void SetupDatabaseUpdateMode(XafApplication app)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && app.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema)
                app.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
#endif
        }
        private static void ApplyXafConfigurations()
        {
            WindowsFormsSettings.LoadApplicationSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
            XpoDefault.TrackPropertiesModifications = true;
            SimpleDataLayer.SuppressReentrancyAndThreadSafetyCheck = true;

            if (Tracing.GetFileLocationFromSettings() == FileLocation.CurrentUserApplicationDataFolder)
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            Tracing.Initialize();
        }
    }
}
