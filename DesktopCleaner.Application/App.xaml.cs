using System.Windows;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.Application.Services;
using DesktopCleaner.Application.Views;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.DataAccessLayer.Db4o;
using GalaSoft.MvvmLight.Messaging;
using log4net.Config;
using Microsoft.Practices.Unity;

namespace DesktopCleaner.Application
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public IUnityContainer Container;
        private IDataAccessLayer _dbContext;

        protected override void OnStartup(StartupEventArgs e)
        {
            BasicConfigurator.Configure();
            Container = new UnityContainer();

            string databaseName = Settings.Default.DatabasePath;

            _dbContext = new Db4OContext();
            _dbContext.Setup(databaseName);

            //database registration
            Container.RegisterInstance(typeof (IDataAccessLayer), _dbContext);

            //service registrations
            Container.RegisterType<IFileSystemService, FileSystemService>();
            Container.RegisterType<IDatabaseService, DatabaseService>();
            Container.RegisterType<IFileWatcherService, FileWatcherService>();

            //registraions utils
            //only one instance from messenger can exists! (recipient problems..)
            var messenger = new Messenger();
            Container.RegisterInstance(typeof (IMessenger), messenger);

            var mainWindow = Container.Resolve<MainWindow>();

            if (Settings.Default.FirstAppStart)
            {
                Settings.Default.FirstAppStart = false;
                mainWindow.Show();
            }
            else
            {
                //start in system tray icon bar
                mainWindow.Hide();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext.Close();

            base.OnExit(e);
        }
    }
}