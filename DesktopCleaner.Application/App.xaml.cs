using System.Windows;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.Application.Services;
using DesktopCleaner.Application.Views;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.DataAccessLayer.Db4o;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using Microsoft.Win32;

namespace DesktopCleaner.Application
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public IUnityContainer Container = new UnityContainer();
        private IDataAccessLayer _dbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            string databaseName = Settings.Default.DatabasePath;

            _dbContext = new Db4OContext();
            _dbContext.Setup(databaseName);

            //database registration
            Container.RegisterInstance(typeof (IDataAccessLayer), _dbContext);

            //service registrations
            //Container.RegisterType<ICategoryService, CategoryService>();
            //Container.RegisterType<IPaymentService, PaymentService>();
            Container.RegisterType<IFileSystemService, FileSystemService>();

            //registraions utils
            //only one instance from messenger can exists! (recipient problems..)
            var messenger = new Messenger();
            Container.RegisterInstance(typeof (IMessenger), messenger);

            if (Settings.Default.FirstAppStart)
            {
                TryToRegisterInAutostart();
            }

            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext.Close();

            base.OnExit(e);
        }

        private void TryToRegisterInAutostart()
        {
            RegistryKey autostartKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (autostartKey != null)
            {
                var executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                autostartKey.SetValue("CAutoStart", executablePath);
                autostartKey.Close();

                Settings.Default.FirstAppStart = false;
            }
        }
    }
}