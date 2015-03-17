using System.IO;
using DesktopCleaner.Application.Messages;
using GalaSoft.MvvmLight.Messaging;
using log4net;

namespace DesktopCleaner.Application.Services
{
    public class FileWatcherService : IFileWatcherService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof (FileWatcherService));

        private readonly IMessenger _messenger;
        private FileSystemWatcher _fileSystemWatcher;
        private bool _isRunning;

        public FileWatcherService(IMessenger messenger)
        {
            _messenger = messenger;
        }

        public bool IsRunning()
        {
            _logger.Info("IsRunning: " + _isRunning);
            return _isRunning;
        }

        public void StartWatching(string pathToDirectory)
        {
            _logger.Info("start watching...");
            _logger.Info("path: " + pathToDirectory);
            _logger.Info("create fileSystemWatcher");

            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = pathToDirectory,
                NotifyFilter = NotifyFilters.LastAccess |
                               NotifyFilters.LastWrite |
                               NotifyFilters.FileName |
                               NotifyFilters.DirectoryName
            };
            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Renamed += OnChanged;
            _fileSystemWatcher.Created += OnChanged;
            _logger.Info("start watcher");
            _fileSystemWatcher.EnableRaisingEvents = true;

            _isRunning = true;
        }

        public void StopWatching()
        {
            _logger.Info("stop watching...");

            _fileSystemWatcher.EnableRaisingEvents = false;
            _fileSystemWatcher.Changed -= OnChanged;
            _fileSystemWatcher.Renamed -= OnChanged;
            _fileSystemWatcher.Created -= OnChanged;
            _fileSystemWatcher.Dispose();
            _fileSystemWatcher = null;

            _logger.Info("fileSystemWatcher: " + _fileSystemWatcher);

            _isRunning = false;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            _logger.Info(string.Format("OnChanged: ({0}) - ({1}) - ({2})", e.ChangeType, e.Name, e.FullPath));
            _logger.Info("send message");
            
            _messenger.Send(e.ChangeType == WatcherChangeTypes.Created
                ? new DirectoryChangedMessage() {DelayClean = true}
                : new DirectoryChangedMessage());
        }
    }
}