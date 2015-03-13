using System;
using System.IO;
using System.Windows.Input;
using DesktopCleaner.Application.Commands;
using DesktopCleaner.Application.Properties;
using DesktopCleaner.Application.Services;
using DesktopCleaner.Application.Views;
using Microsoft.Practices.Unity;

namespace DesktopCleaner.Application.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IFileSystemService _fileSystemService;
        private ICommand _cleanCommand;
        private ICommand _configurationCommand;
        private ICommand _setupCommand;

        public MainWindowViewModel(IFileSystemService fileSystemService)
        {
            _fileSystemService = fileSystemService;
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
            throw new NotImplementedException();
        }

        private void Clean(object obj)
        {
            //TODO: blacklisted files/ dirs berücksichtigen!!!!!!!!

            string destionationPath = Settings.Default.DestinationPath;

            if (!Directory.Exists(destionationPath))
            {
                Directory.CreateDirectory(destionationPath);
            }

            CopyFiles();
            CopyDirectories();
        }

        private void CopyFiles()
        {
            _fileSystemService.CopyFiles();
        }

        private void CopyDirectories()
        {
            _fileSystemService.CopyDirectories();
        }
    }
}