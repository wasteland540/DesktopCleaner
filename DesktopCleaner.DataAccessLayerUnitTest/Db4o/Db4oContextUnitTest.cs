using System;
using System.Collections.Generic;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.DataAccessLayer.Db4o;
using DesktopCleaner.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DesktopCleaner.DataAccessLayerUnitTest.Db4o
{
    /// <summary>
    ///     Summary description for Db4oContextUnitTest
    /// </summary>
    [TestClass]
    public class Db4OContextUnitTest
    {
        private const string DatabaseName = "C:/test/DB4O/desktopcleaner/database.yap";
        private static IDataAccessLayer _context;

        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize]
        public static void MyClassInitialize(TestContext testContext)
        {
            _context = new Db4OContext();
            _context.Setup(DatabaseName);
            _context.ClearDb(DatabaseName);
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup]
        public static void MyClassCleanup()
        {
            _context.ClearDb(DatabaseName);
            _context = null;
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup]
        public void MyTestCleanup()
        {
            _context.ClearDb(DatabaseName);
        }

        [TestMethod]
        public void InsertSingle()
        {
            var blacklistedFile = new BlackListedFile
            {
                FileName = "*",
                FileExtension = "lnk"
            };

            string id = _context.Insert(DatabaseName, blacklistedFile);
            Assert.IsTrue(id != Guid.Empty.ToString());

            var blacklistedDirectory = new BlackListedDirectory
            {
                Name = "test"
            };

            id = _context.Insert(DatabaseName, blacklistedDirectory);
            Assert.IsTrue(id != Guid.Empty.ToString());
        }

        [TestMethod]
        public void UpdateSingle()
        {
            var blacklistedFile = new BlackListedFile
            {
                FileName = "*",
                FileExtension = "lnk"
            };

            string id = _context.Insert(DatabaseName, blacklistedFile);
            Assert.IsTrue(id != Guid.Empty.ToString());

            blacklistedFile = _context.GetEntry<BlackListedFile>(DatabaseName, id);
            Assert.IsTrue(blacklistedFile != null);
            Assert.IsTrue(blacklistedFile.FileName == "*");
            Assert.IsTrue(blacklistedFile.FileExtension == "lnk");

            //update
            blacklistedFile.FileName = "*_bak";
            _context.Update(DatabaseName, blacklistedFile);

            blacklistedFile = _context.GetEntry<BlackListedFile>(DatabaseName, id);
            Assert.IsTrue(blacklistedFile != null);
            Assert.IsTrue(blacklistedFile.FileName == "*_bak");
            Assert.IsTrue(blacklistedFile.FileExtension == "lnk");
        }

        [TestMethod]
        public void DeleteSingle()
        {
            var blacklistedFile = new BlackListedFile
            {
                FileName = "*",
                FileExtension = "lnk"
            };

            string id = _context.Insert(DatabaseName, blacklistedFile);
            Assert.IsTrue(id != Guid.Empty.ToString());

            blacklistedFile = _context.GetEntry<BlackListedFile>(DatabaseName, id);

            //delete
            _context.Delete(DatabaseName, blacklistedFile);
            blacklistedFile = _context.GetEntry<BlackListedFile>(DatabaseName, id);
            Assert.IsTrue(blacklistedFile == null);
        }

        [TestMethod]
        public void GetBlacklistedFile()
        {
            var blacklistedFile = new BlackListedFile
            {
                FileName = "*",
                FileExtension = "lnk"
            };

            string id = _context.Insert(DatabaseName, blacklistedFile);
            Assert.IsTrue(id != Guid.Empty.ToString());

            blacklistedFile = _context.GetEntry<BlackListedFile>(DatabaseName, id);
            Assert.IsTrue(blacklistedFile != null);
            Assert.IsTrue(blacklistedFile.FileName == "*");
            Assert.IsTrue(blacklistedFile.FileExtension == "lnk");
        }

        [TestMethod]
        public void GetBlacklistedDirectory()
        {
            var blacklistedDirectory = new BlackListedDirectory
            {
                Name = "test"
            };

            string id = _context.Insert(DatabaseName, blacklistedDirectory);
            Assert.IsTrue(id != Guid.Empty.ToString());

            blacklistedDirectory = _context.GetEntry<BlackListedDirectory>(DatabaseName, id);
            Assert.IsTrue(blacklistedDirectory != null);
            Assert.IsTrue(blacklistedDirectory.Name == "test");
        }

        [TestMethod]
        public void GetBlacklistedFiles()
        {
            var blacklistedFile = new BlackListedFile
            {
                FileName = "*",
                FileExtension = "lnk"
            };

            var blacklistedFile2 = new BlackListedFile
            {
                FileName = "*",
                FileExtension = "exe"
            };

            string id = _context.Insert(DatabaseName, blacklistedFile);
            Assert.IsTrue(id != Guid.Empty.ToString());

            id = _context.Insert(DatabaseName, blacklistedFile2);
            Assert.IsTrue(id != Guid.Empty.ToString());

            List<BlackListedFile> blacklistedFiles = _context.GetEntries<BlackListedFile>(DatabaseName);
            Assert.IsTrue(blacklistedFiles != null);
            Assert.IsTrue(blacklistedFiles.Count == 2);
        }

        [TestMethod]
        public void GetBlacklistedDirectories()
        {
            var blacklistedDirectory = new BlackListedDirectory
            {
                Name = "test"
            };

            var blacklistedDirectory2 = new BlackListedDirectory
            {
                Name = "test2"
            };

            string id = _context.Insert(DatabaseName, blacklistedDirectory);
            Assert.IsTrue(id != Guid.Empty.ToString());

            id = _context.Insert(DatabaseName, blacklistedDirectory2);
            Assert.IsTrue(id != Guid.Empty.ToString());

            List<BlackListedDirectory> blacklistedDirectories = _context.GetEntries<BlackListedDirectory>(DatabaseName);
            Assert.IsTrue(blacklistedDirectories != null);
            Assert.IsTrue(blacklistedDirectories.Count == 2);
        }

        [TestMethod]
        public void GetBlacklistedFilesByIdList()
        {
            var idList = new List<string>();

            var blacklistedFile = new BlackListedFile
            {
                FileName = "*",
                FileExtension = "lnk"
            };

            var blacklistedFile2 = new BlackListedFile
            {
                FileName = "*",
                FileExtension = "exe"
            };

            string id = _context.Insert(DatabaseName, blacklistedFile);
            Assert.IsTrue(id != Guid.Empty.ToString());
            idList.Add(id);

            id = _context.Insert(DatabaseName, blacklistedFile2);
            Assert.IsTrue(id != Guid.Empty.ToString());
            idList.Add(id);

            Assert.IsTrue(idList.Count == 2);

            List<BlackListedFile> blacklistedFiles = _context.GetEntries<BlackListedFile>(DatabaseName, idList);
            Assert.IsTrue(blacklistedFiles != null);
            Assert.IsTrue(blacklistedFiles.Count == 2);
        }

        [TestMethod]
        public void GetBlacklistedDirectoriesByIdList()
        {
            var idList = new List<string>();

            var blacklistedDirectory = new BlackListedDirectory
            {
                Name = "test"
            };

            var blacklistedDirectory2 = new BlackListedDirectory
            {
                Name = "test2"
            };

            string id = _context.Insert(DatabaseName, blacklistedDirectory);
            Assert.IsTrue(id != Guid.Empty.ToString());
            idList.Add(id);

            id = _context.Insert(DatabaseName, blacklistedDirectory2);
            Assert.IsTrue(id != Guid.Empty.ToString());
            idList.Add(id);

            Assert.IsTrue(idList.Count == 2);

            List<BlackListedDirectory> blacklistedDirectories = _context.GetEntries<BlackListedDirectory>(DatabaseName,
                idList);
            Assert.IsTrue(blacklistedDirectories != null);
            Assert.IsTrue(blacklistedDirectories.Count == 2);
        }
    }
}