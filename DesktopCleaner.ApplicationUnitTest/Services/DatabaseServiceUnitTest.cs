using System.Collections.Generic;
using DesktopCleaner.Application.Services;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesktopCleaner.ApplicationUnitTest.Services
{
    /// <summary>
    ///     Summary description for DatabaseServiceTest
    /// </summary>
    [TestClass]
    public class DatabaseServiceUnitTest
    {
        private const string DatabaseName = "C:/test/DB4O/desktopcleaner/database.yap";
        private static IDataAccessLayer _context;
        private static IUnityContainer _container;

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            _container = DIContainerHelper.Setup(out _context);
            _context.ClearDb(DatabaseName);
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
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MyTestCleanup()
        {
            _context.ClearDb(DatabaseName);
        }

        [TestMethod]
        public void AddAndGetBlacklistedFiles()
        {
            var databaseService = _container.Resolve<IDatabaseService>();
            Assert.IsNotNull(databaseService);

            databaseService.SetDatabaseName(DatabaseName);
            List<BlackListedFile> blacklistedFiles = databaseService.GetBlackListedFiles();
            Assert.IsTrue(blacklistedFiles.Count == 0);

            var blacklistedFile = new BlackListedFile
            {
                FileName = "*",
                FileExtension = ".zip"
            };

            databaseService.AddBlacklistedFile(blacklistedFile);
            blacklistedFiles = databaseService.GetBlackListedFiles();
            Assert.IsTrue(blacklistedFiles.Count == 1);
            Assert.IsTrue(blacklistedFiles[0].FileName == "*");
            Assert.IsTrue(blacklistedFiles[0].FileExtension == ".zip");
        }

        [TestMethod]
        public void AddAndGetBlacklistedDirectories()
        {
            var databaseService = _container.Resolve<IDatabaseService>();
            Assert.IsNotNull(databaseService);

            databaseService.SetDatabaseName(DatabaseName);
            List<BlackListedDirectory> blacklistedDirectories = databaseService.GetBlackListedDirectories();
            Assert.IsTrue(blacklistedDirectories.Count == 0);

            var blacklistedDirectory = new BlackListedDirectory
            {
                Name = "VIP"
            };

            databaseService.AddBlacklistedDirectory(blacklistedDirectory);
            blacklistedDirectories = databaseService.GetBlackListedDirectories();
            Assert.IsTrue(blacklistedDirectories.Count == 1);
            Assert.IsTrue(blacklistedDirectories[0].Name == "VIP");
        }

        [TestMethod]
        public void RemoveBlacklistedFile()
        {
            var databaseService = _container.Resolve<IDatabaseService>();
            Assert.IsNotNull(databaseService);

            databaseService.SetDatabaseName(DatabaseName);
            List<BlackListedFile> blacklistedFiles = databaseService.GetBlackListedFiles();
            Assert.IsTrue(blacklistedFiles.Count == 0);

            var blacklistedFile = new BlackListedFile
            {
                FileName = "*",
                FileExtension = ".zip"
            };

            databaseService.AddBlacklistedFile(blacklistedFile);
            blacklistedFiles = databaseService.GetBlackListedFiles();
            Assert.IsTrue(blacklistedFiles.Count == 1);
            Assert.IsTrue(blacklistedFiles[0].FileName == "*");
            Assert.IsTrue(blacklistedFiles[0].FileExtension == ".zip");

            databaseService.RemoveBlacklistedFile(blacklistedFiles[0]);
            blacklistedFiles = databaseService.GetBlackListedFiles();
            Assert.IsTrue(blacklistedFiles.Count == 0);
        }

        [TestMethod]
        public void RemoveBlacklistedDirectory()
        {
            var databaseService = _container.Resolve<IDatabaseService>();
            Assert.IsNotNull(databaseService);

            databaseService.SetDatabaseName(DatabaseName);
            List<BlackListedDirectory> blacklistedDirectories = databaseService.GetBlackListedDirectories();
            Assert.IsTrue(blacklistedDirectories.Count == 0);

            var blacklistedDirectory = new BlackListedDirectory
            {
                Name = "VIP"
            };

            databaseService.AddBlacklistedDirectory(blacklistedDirectory);
            blacklistedDirectories = databaseService.GetBlackListedDirectories();
            Assert.IsTrue(blacklistedDirectories.Count == 1);
            Assert.IsTrue(blacklistedDirectories[0].Name == "VIP");

            databaseService.RemoveBlacklistedDirectory(blacklistedDirectories[0]);
            blacklistedDirectories = databaseService.GetBlackListedDirectories();
            Assert.IsTrue(blacklistedDirectories.Count == 0);
        }
    }
}