using System.Windows.Forms;
using System.Windows.Input;
using DesktopCleaner.Application.Commands;
using DesktopCleaner.Application.Properties;

namespace DesktopCleaner.Application.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        private ICommand _chooseCommand;
        private string _destinationPath = Settings.Default.DestinationPath;

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
    }
}