using System.IO;
using System.Linq;
using DesktopCleaner.Application.Services;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.Model;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesktopCleaner.ApplicationUnitTest.Services
{
    /// <summary>
    ///     Summary description for FileSystemServiceUnitTest
    /// </summary>
    [TestClass]
    public class FileSystemServiceUnitTest
    {
        private const string DesktopTestDirectory = "C:/test/DESKTOPCLEANER/Desktop";
        private const string DestinationDirectory = "C:/DesktopCleaner/";
        private const string DatabaseName = "C:/test/DB4O/desktopcleaner/database.yap";
        private static IDataAccessLayer _context;
        private static IUnityContainer _container;

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

            if (!Directory.Exists(DestinationDirectory))
            {
                Directory.CreateDirectory(DestinationDirectory);
            }

            //create test files on the 'desktop'
            File.Create(Path.Combine(DesktopTestDirectory, "test.txt")).Close();
            File.Create(Path.Combine(DesktopTestDirectory, "test1.zip")).Close();
            File.Create(Path.Combine(DesktopTestDirectory, "test2.zip")).Close();
            File.Create(Path.Combine(DesktopTestDirectory, "test3.xml")).Close();
            File.Create(Path.Combine(DesktopTestDirectory, "test4.xml")).Close();
            File.Create(Path.Combine(DesktopTestDirectory, "test5.xml")).Close();
            
            //simulates a shortcut on the desktop
            File.Create(Path.Combine(DesktopTestDirectory, "Desktop.lnk")).Close();

            //create test directories
            var dir1Path = Path.Combine(DesktopTestDirectory, "dir1");
            Directory.CreateDirectory(dir1Path);
            File.Create(Path.Combine(dir1Path, "dirTest1.txt")).Close();

            var dir2Path = Path.Combine(DesktopTestDirectory, "dir2");
            Directory.CreateDirectory(dir2Path);
            File.Create(Path.Combine(dir2Path, "dirTest2.txt")).Close();

            var vipDirPath = Path.Combine(DesktopTestDirectory, "VIPdir");
            Directory.CreateDirectory(vipDirPath);
            File.Create(Path.Combine(vipDirPath, "vipText.xml")).Close();

            //configure blacklisted files and directories
            var blacklistedFile1 = new BlackListedFile
            {
                FileName = "test",
                FileExtension = ".txt"
            };

            var blacklistedFile2 = new BlackListedFile
            {
                FileName = "*",
                FileExtension = ".zip"
            };

            var blacklistedDir = new BlackListedDirectory
            {
                Name = "VIPdir"
            };

            var databaseService = _container.Resolve<IDatabaseService>();
            databaseService.AddBlacklistedFile(blacklistedFile1);
            databaseService.AddBlacklistedFile(blacklistedFile2);
            databaseService.AddBlacklistedDirectory(blacklistedDir);
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
            Directory.Delete(DestinationDirectory, true);
        }

        [TestMethod]
        public void CopyFiles()
        {
            var filesystemService = _container.Resolve<IFileSystemService>();
            Assert.IsNotNull(filesystemService);
            filesystemService.SetDesktopDirectory(DesktopTestDirectory);

            filesystemService.CopyFiles();

            var files = Directory.GetFiles(DesktopTestDirectory);
            Assert.IsTrue(files.Length == 4); //all files except the xml files

            files = Directory.GetFiles(DestinationDirectory);
            Assert.IsTrue(files.Length == 3); //all xml files
            Assert.IsTrue(files.All(f => f.EndsWith(".xml")));
        }

        [TestMethod]
        public void CopyDirectories()
        {
            var filesystemService = _container.Resolve<IFileSystemService>();
            Assert.IsNotNull(filesystemService);
            filesystemService.SetDesktopDirectory(DesktopTestDirectory);

            filesystemService.CopyDirectories();

            var dirs = Directory.GetDirectories(DesktopTestDirectory);
            Assert.IsTrue(dirs.Length == 1);

            dirs = Directory.GetDirectories(DestinationDirectory);
            Assert.IsTrue(dirs.Length == 2);
        }

    }
}