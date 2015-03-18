using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using DesktopCleaner.Application.Messages;
using DesktopCleaner.Application.Services;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.Model;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesktopCleaner.ApplicationUnitTest.Services
{
    /// <summary>
    /// Summary description for FileWatcherServiceUnitTest
    /// </summary>
    [TestClass]
    public class FileWatcherServiceUnitTest
    {
        private const string DesktopTestDirectory = "C:/test/DESKTOPCLEANER/Desktop";
        private const string DatabaseName = "C:/test/DB4O/desktopcleaner/database.yap";
        private static IDataAccessLayer _context;
        private static IUnityContainer _container;
        private bool _messageDelivered;

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            _container = DIContainerHelper.Setup(out _context);
            _context.ClearDb(DatabaseName);

            //create test directory, which simulates the desktop folder
            if (!Directory.Exists(DesktopTestDirectory))
            {
                Directory.CreateDirectory(DesktopTestDirectory);
            }
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            _context.ClearDb(DatabaseName);
            _context.Close();
            _context = null;

            _container.Dispose();
            _container = null;

            Directory.Delete(DesktopTestDirectory, true);
        }

        [TestMethod]
        public void IsRunning()
        {
            var filewatcherService = _container.Resolve<IFileWatcherService>();
            Assert.IsNotNull(filewatcherService);

            Assert.IsFalse(filewatcherService.IsRunning());

            filewatcherService.StartWatching(DesktopTestDirectory);
            Assert.IsTrue(filewatcherService.IsRunning());

            filewatcherService.StopWatching();
            Assert.IsFalse(filewatcherService.IsRunning());
        }

        [TestMethod]
        public async Task Watching()
        {
            var messenger = _container.Resolve<IMessenger>();
            messenger.Register<DirectoryChangedMessage>(this, OnDirectoryChangedMessageReceive);

            var filewatcherService = _container.Resolve<IFileWatcherService>();
            Assert.IsNotNull(filewatcherService);

            Assert.IsFalse(filewatcherService.IsRunning());

            filewatcherService.StartWatching(DesktopTestDirectory);
            Assert.IsTrue(filewatcherService.IsRunning());

            //create test files on the 'desktop'
            File.Create(Path.Combine(DesktopTestDirectory, "test.txt")).Close();

            //wait for message (i think its a dirty way, but i don't know a better solution at the moment...)
            await Task.Delay(5000);

            Assert.IsTrue(_messageDelivered);

            filewatcherService.StopWatching();
            Assert.IsFalse(filewatcherService.IsRunning());
        }

        private void OnDirectoryChangedMessageReceive(DirectoryChangedMessage obj)
        {
            Assert.IsNotNull(obj);
            _messageDelivered = true;
        }
    }
}
