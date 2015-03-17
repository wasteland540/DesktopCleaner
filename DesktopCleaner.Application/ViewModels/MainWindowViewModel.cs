using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using DesktopCleaner.Application.Commands;
using DesktopCleaner.Application.Messages;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.Application.Services;
using DesktopCleaner.Application.Views;
using GalaSoft.MvvmLight.Messaging;
using log4net;
using Microsoft.Practices.Unity;

namespace DesktopCleaner.Application.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(MainWindowViewModel));

        private readonly IFileSystemService _fileSystemService;
        private readonly IFileWatcherService _fileWatcherService;
        private ICommand _cleanCommand;
        private ICommand _configurationCommand;
        private ICommand _setupCommand;

        public MainWindowViewModel(IFileSystemService fileSystemService, IFileWatcherService fileWatcherService, IMessenger messenger)
        {
            _fileSystemService = fileSystemService;
            _fileWatcherService = fileWatcherService;

            _logger.Info("register on directoryChangedMessage");
            messenger.Register<DirectoryChangedMessage>(this, OnDirectoryChangedMessageReceive);
        }
        
        public ICommand ConfigurationCommand
        {
            get
            {
                _configurationCommand = _configurationCommand ?? new DelegateCommand(OpenConfigurationView);
                return _configurationCommand;
            }
        }

        public ICommand SetupCommand
        {
            get
            {
                _setupCommand = _setupCommand ?? new DelegateCommand(Setup);
                return _setupCommand;
            }
        }

        public ICommand CleanCommand
        {
            get
            {
                _cleanCommand = _cleanCommand ?? new DelegateCommand(Clean);
                return _cleanCommand;
            }
        }

        private void OpenConfigurationView(object obj)
        {
            var configurationView = Container.Resolve<ConfigurationView>();
            configurationView.ShowDialog();
        }

        private void Setup(object obj)
        {
            _logger.Info("start Setup...");

            _logger.Info("Clean");
            Clean(null);

            _logger.Info("Is filewatcher running?");
            if (_fileWatcherService.IsRunning())
            {
                _logger.Info("filewater is running. stop it!");
                _fileWatcherService.StopWatching();
                _logger.Info("is it still runnning after stop: " + _fileWatcherService.IsRunning());
            }

            _logger.Info("start file watcher");
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            _fileWatcherService.StartWatching(desktopPath);
        }

        private void Clean(object obj)
        {
            _logger.Info("start clean...");
            string destinationPath = Settings.Default.DestinationPath;
            _logger.Info("destinationPath: " + destinationPath);

            if (!Directory.Exists(destinationPath))
            {
                _logger.Info("destination path do not exists --> create it!");
                Directory.CreateDirectory(destinationPath);
            }

            CopyFiles();
            CopyDirectories();
        }

        private void CopyFiles()
        {
            _logger.Info("start CopyFiles...");
            _fileSystemService.CopyFiles();
        }

        private void CopyDirectories()
        {
            _logger.Info("start CopyDirectories...");
            _fileSystemService.CopyDirectories();
        }

        private async void OnDirectoryChangedMessageReceive(DirectoryChangedMessage obj)
        {
            _logger.Info("received message of type 'DirectoryChangedMessage'. --> Clean(null)");
            _logger.Info("delay clean: " + obj.DelayClean);

            if (obj.DelayClean)
            {
                _logger.Info("delay for 20 sec.");
                await Task.Delay(20000);
            }

            _logger.Info("execute Clean(null)");
            Clean(null);
        }
    }
}