using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Input;
using DesktopCleaner.Application.Commands;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.Application.Services;
using DesktopCleaner.Model;

namespace DesktopCleaner.Application.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private readonly IDatabaseService _databaseService;
        private ICommand _addFileCommand;
        private ICommand _addFolderCommand;
        private List<BlackListedDirectory> _blackListedDirectories;
        private List<BlackListedFile> _blackListedFiles;
        private ICommand _chooseCommand;
        private string _destinationPath = Settings.Default.DestinationPath;
        private string _newExtension;
        private string _newFilename = "*";
        private string _newFolder;
        private ICommand _removeFileCommand;
        private ICommand _removeFolderCommand;
        private BlackListedDirectory _selectedBlackListedDirectory;
        private BlackListedFile _selectedBlackListedFile;

        public ConfigurationViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public string DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                if (value != null && value != _destinationPath)
                {
                    _destinationPath = value;
                    Settings.Default.DestinationPath = _destinationPath;
                    Settings.Default.Save();

                    RaisePropertyChanged("DestinationPath");
                }
            }
        }

        public ICommand ChooseCommand
        {
            get
            {
                _chooseCommand = _chooseCommand ?? new DelegateCommand(ChooseDestinatonPath);
                return _chooseCommand;
            }
        }

        public string NewFilename
        {
            get { return _newFilename; }
            set
            {
                if (value != null && value != _newFilename)
                {
                    _newFilename = value;

                    RaisePropertyChanged("NewFilename");
                }
            }
        }

        public string NewExtension
        {
            get { return _newExtension; }
            set
            {
                if (value != null && value != _newExtension)
                {
                    _newExtension = value;

                    RaisePropertyChanged("NewExtension");
                }
            }
        }

        public BlackListedFile SelectedBlackListedFile
        {
            get { return _selectedBlackListedFile; }
            set
            {
                if (value != null && value != _selectedBlackListedFile)
                {
                    _selectedBlackListedFile = value;

                    RaisePropertyChanged("SelectedBlackListedFile");
                }
            }
        }

        public string NewFolder
        {
            get { return _newFolder; }
            set
            {
                if (value != null && value != _newFolder)
                {
                    _newFolder = value;

                    RaisePropertyChanged("NewFolder");
                }
            }
        }

        public BlackListedDirectory SelectedBlackListedDirectory
        {
            get { return _selectedBlackListedDirectory; }
            set
            {
                if (value != null && value != _selectedBlackListedDirectory)
                {
                    _selectedBlackListedDirectory = value;

                    RaisePropertyChanged("SelectedBlackListedDirectory");
                }
            }
        }

        public List<BlackListedFile> BlackListedFiles
        {
            get
            {
                _blackListedFiles = _databaseService.GetBlackListedFiles();

                return _blackListedFiles;
            }

            set
            {
                if (value != null && value != _blackListedFiles)
                {
                    _blackListedFiles = value;

                    RaisePropertyChanged("BlackListedFiles");
                }
            }
        }

        public List<BlackListedDirectory> BlackListedDirectories
        {
            get
            {
                _blackListedDirectories = _databaseService.GetBlackListedDirectories();

                return _blackListedDirectories;
            }

            set
            {
                if (value != null && value != _blackListedDirectories)
                {
                    _blackListedDirectories = value;

                    RaisePropertyChanged("BlackListedDirectories");
                }
            }
        }

        public ICommand AddFileCommand
        {
            get
            {
                _addFileCommand = _addFileCommand ?? new DelegateCommand(AddFile);
                return _addFileCommand;
            }
        }

        public ICommand RemoveFileCommand
        {
            get
            {
                _removeFileCommand = _removeFileCommand ?? new DelegateCommand(RemoveFile);
                return _removeFileCommand;
            }
        }

        public ICommand AddFolderCommand
        {
            get
            {
                _addFolderCommand = _addFolderCommand ?? new DelegateCommand(AddFolder);
                return _addFolderCommand;
            }
        }

        public ICommand RemoveFolderCommand
        {
            get
            {
                _removeFolderCommand = _removeFolderCommand ?? new DelegateCommand(RemoveFolder);
                return _removeFolderCommand;
            }
        }

        private void ChooseDestinatonPath(object obj)
        {
            //open file chooser
            var openFolderDialog = new FolderBrowserDialog();
            DialogResult dialogResult = openFolderDialog.ShowDialog();

            if (dialogResult == DialogResult.OK)
            {
                DestinationPath = openFolderDialog.SelectedPath;
            }
        }

        private void AddFile(object obj)
        {
            bool isAlreadyListed =
                _blackListedFiles.Any(f => f.FileExtension == _newExtension && f.FileName == _newFilename);

            if (!isAlreadyListed)
            {
                if (!string.IsNullOrEmpty(_newExtension))
                {
                    var blackFile = new BlackListedFile
                    {
                        FileName = _newFilename,
                        FileExtension = "." + _newExtension,
                    };

                    _databaseService.AddBlacklistedFile(blackFile);

                    BlackListedFiles = _databaseService.GetBlackListedFiles();

                    NewFilename = "*";
                    NewExtension = string.Empty;

                    MessageBox.Show(Resources.ConfigurationViewModel_AddFile_File_added_to_Blacklist_,
                        Resources.ConfigurationViewModel_AddFile_Info, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(
                        Resources.ConfigurationViewModel_AddFile_File_not_added_to_Blacklist__Extension_cannot_be_empty_,
                        Resources.ConfigurationViewModel_AddFile_Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void RemoveFile(object obj)
        {
            _databaseService.RemoveBlacklistedFile(_selectedBlackListedFile);

            BlackListedFiles = _databaseService.GetBlackListedFiles();
        }

        private void AddFolder(object obj)
        {
            bool isAlreadyListed = _blackListedDirectories.Any(f => f.Name == _newFolder);

            if (!isAlreadyListed)
            {
                if (!string.IsNullOrEmpty(_newFolder))
                {
                    var blackFolder = new BlackListedDirectory
                    {
                        Name = _newFolder
                    };

                    _databaseService.AddBlacklistedDirectory(blackFolder);

                    BlackListedDirectories = _databaseService.GetBlackListedDirectories();

                    NewFolder = string.Empty;
                }
            }
        }

        private void RemoveFolder(object obj)
        {
            _databaseService.RemoveBlacklistedDirectory(_selectedBlackListedDirectory);

            BlackListedDirectories = _databaseService.GetBlackListedDirectories();
        }
    }
}