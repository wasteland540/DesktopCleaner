using System.Collections.Generic;
using System.Linq;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.DataAccessLayer;
using DesktopCleaner.Model;

namespace DesktopCleaner.Application.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly IDataAccessLayer _context;
        private readonly string _databaseName = Settings.Default.DatabasePath;

        public DatabaseService(IDataAccessLayer context)
        {
            _context = context;
        }

        public List<BlackListedFile> GetBlackListedFiles()
        {
            List<BlackListedFile> blacklistedFiles = _context.GetEntries<BlackListedFile>(_databaseName);

            return blacklistedFiles.ToList();
        }

        public List<BlackListedDirectory> GetBlackListedDirectories()
        {
            List<BlackListedDirectory> blacklistedDirectories = _context.GetEntries<BlackListedDirectory>(_databaseName);

            return blacklistedDirectories.ToList();
        }

        public void AddBlacklistedFile(BlackListedFile blackListedFile)
        {
            _context.Insert(_databaseName, blackListedFile);
        }

        public void AddBlacklistedDirectory(BlackListedDirectory blackListedDirectory)
        {
            _context.Insert(_databaseName, blackListedDirectory);
        }

        public void RemoveBlacklistedFile(BlackListedFile blackListedFile)
        {
            _context.Delete(_databaseName, blackListedFile);
        }

        public void RemoveBlacklistedDirectory(BlackListedDirectory blackListedDirectory)
        {
            _context.Delete(_databaseName, blackListedDirectory);
        }
    }
}