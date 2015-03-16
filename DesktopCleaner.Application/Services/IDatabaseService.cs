using System.Collections.Generic;
using DesktopCleaner.Model;

namespace DesktopCleaner.Application.Services
{
    public interface IDatabaseService
    {
        List<BlackListedFile> GetBlackListedFiles();

        List<BlackListedDirectory> GetBlackListedDirectories();

        void AddBlacklistedFile(BlackListedFile blackListedFile);

        void AddBlacklistedDirectory(BlackListedDirectory blackListedDirectory);

        void RemoveBlacklistedFile(BlackListedFile blackListedFile);

        void RemoveBlacklistedDirectory(BlackListedDirectory blackListedDirectory);
    }
}