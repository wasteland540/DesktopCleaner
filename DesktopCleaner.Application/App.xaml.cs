using System.Windows;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.Application.Views;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.DataAccessLayer.Db4o;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;

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

            //registraions utils
            //only one instance from messenger can exists! (recipient problems..)
            var messenger = new Messenger();
            Container.RegisterInstance(typeof (IMessenger), messenger);

            var mainWindow = Container.Resolve<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _dbContext.Close();

            base.OnExit(e);
        }
    }
}