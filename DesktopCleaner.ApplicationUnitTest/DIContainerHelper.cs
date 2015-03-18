using DesktopCleaner.Application.Services;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.DataAccessLayer.Db4o;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;

namespace DesktopCleaner.ApplicationUnitTest
{
    // ReSharper disable once InconsistentNaming
    public class DIContainerHelper
    {
        public const string DatabasePath = @"C:\test\DB4O\desktopcleaner\database.yap";

        public static IUnityContainer Setup(out IDataAccessLayer dbContext)
        {
            var container = new UnityContainer();

            const string databaseName = DatabasePath;

            dbContext = new Db4OContext();
            dbContext.Setup(databaseName);

            //database registration
            container.RegisterInstance(typeof (IDataAccessLayer), dbContext);

            //service registrations
            container.RegisterType<IFileSystemService, FileSystemService>();
            container.RegisterType<IDatabaseService, DatabaseService>();
            container.RegisterType<IFileWatcherService, FileWatcherService>();

            //registraions utils
            //only one instance from messenger can exists! (recipient problems..)
            var messenger = new Messenger();
            container.RegisterInstance(typeof (IMessenger), messenger);

            return container;
        }
    }
}