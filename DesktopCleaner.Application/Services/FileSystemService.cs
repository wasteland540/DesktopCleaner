using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.Model;

namespace DesktopCleaner.Application.Services
{
    public class FileSystemService : IFileSystemService
    {
        private static string _desktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

        private static readonly BlackListedFile LnkBlackListedFile = new BlackListedFile
        {
            FileExtension = ".lnk",
            FileName = "*"
        };

        private readonly IDatabaseService _databaseService;

        public FileSystemService(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }
        
        //only for unit test
        public void SetDesktopDirectory(string path)
        {
            _desktopDirectory = path;
        }

        public void CopyFiles()
        {
            string destionationPath = Settings.Default.DestinationPath;
            string[] files = Directory.GetFiles(_desktopDirectory);
            
            foreach (string file in files)
            {
                string filename = Path.GetFileName(file);

                if (filename != null)
                {
                    if (!IsBlacklistedFile(filename))
                    {
                        string destinationFile = Path.Combine(destionationPath, filename);

                        if (File.Exists(destinationFile))
                        {
                            int doubleCounter = Settings.Default.DoubleFileExtension;

                            string extension = Path.GetExtension(filename);
                            string name = filename.Replace(extension, "");

                            name = name + "{" + doubleCounter + "}" + extension;

                            destinationFile = Path.Combine(destionationPath, name);

                            doubleCounter++;
                            Settings.Default.DoubleFileExtension = doubleCounter;
                            Settings.Default.Save();
                        }

                        var info = new FileInfo(file);
                        info.MoveTo(destinationFile);
                    }
                }
            }
        }

        public void CopyDirectories()
        {
            string destinationPath = Settings.Default.DestinationPath;
            string[] directories = Directory.GetDirectories(_desktopDirectory);

            foreach (string directory in directories)
            {
                string dirName = Path.GetFileName(directory);

                if (dirName != null)
                {
                    if (!IsBlacklistedDir(dirName))
                    {
                        string destinationDirectory = Path.Combine(destinationPath, dirName);

                        if (Directory.Exists(destinationDirectory))
                        {
                            int doubleCounter = Settings.Default.DoubleFileExtension;

                            string name = dirName;
                            name = name + "{" + doubleCounter + "}";

                            destinationDirectory = Path.Combine(destinationPath, name);

                            doubleCounter++;
                            Settings.Default.DoubleFileExtension = doubleCounter;
                        }

                        var info = new DirectoryInfo(directory);
                        info.MoveTo(destinationDirectory);
                    }
                }
            }
        }

        private bool IsBlacklistedFile(string filename)
        {
            bool isBlacklisted = false;

            string extension = Path.GetExtension(filename);

            if (extension == LnkBlackListedFile.FileExtension)
            {
                isBlacklisted = true;
            }
            else
            {
                List<BlackListedFile> blacklistedFiles = _databaseService.GetBlackListedFiles();

                foreach (BlackListedFile blacklistedFile in blacklistedFiles)
                {
                    if (blacklistedFile.FileName == "*")
                    {
                        if (extension == blacklistedFile.FileExtension)
                        {
                            isBlacklisted = true;
                            break;
                        }
                    }
                    else
                    {
                        string name = Path.GetFileName(filename);

                        if (extension != null && name != null)
                        {
                            name = name.Replace(extension, "");
                        
                            if (name == blacklistedFile.FileName && extension == blacklistedFile.FileExtension)
                            {
                                isBlacklisted = true;
                                break;
                            }
                        }
                    }
                }
            }

            return isBlacklisted;
        }

        private bool IsBlacklistedDir(string dirName)
        {
            List<BlackListedDirectory> blacklistedDirectories = _databaseService.GetBlackListedDirectories();

            return blacklistedDirectories.Any(blacklistedDirectory => blacklistedDirectory.Name == dirName);
        }
    }
}