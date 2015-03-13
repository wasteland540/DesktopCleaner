using System;
using System.IO;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.Model;

namespace DesktopCleaner.Application.Services
{
    public class FileSystemService : IFileSystemService
    {
        private static readonly string DesktopDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        private static readonly BlackListedFile LnkBlackListedFile = new BlackListedFile{FileExtension = ".lnk", FileName = "*"};

        public void CopyFiles()
        {
            string destionationPath = Settings.Default.DestinationPath;
            string[] files = Directory.GetFiles(DesktopDirectory);

            foreach (string file in files)
            {
                string filename = Path.GetFileName(file);

                if (filename != null)
                {
                    if (!IsBlacklisted(filename))
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
            string[] directories = Directory.GetDirectories(DesktopDirectory);

            foreach (string directory in directories)
            {
                string dirName = Path.GetFileName(directory);

                if (dirName != null)
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

        private bool IsBlacklisted(string filename)
        {
            bool isBlacklisted = false;

            string extension = Path.GetExtension(filename);

            if (extension == LnkBlackListedFile.FileExtension)
            {
                isBlacklisted = true;
            }
            
            //TODO: configurierte blacklisted files checken

            return isBlacklisted;
        }
    }
}