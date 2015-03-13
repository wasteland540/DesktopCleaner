using System.Windows;
using DesktopCleaner.Application.ViewModels;
using Microsoft.Practices.Unity;

namespace DesktopCleaner.Application.Views
{
    /// <summary>
    ///     Interaction logic for ConfigurationView.xaml
    /// </summary>
    public partial class ConfigurationView : Window
    {
        public ConfigurationView()
        {
            InitializeComponent();
        }

        [Dependency]
        public ConfigurationViewModel ViewModel
        {
            set { DataContext = value; }
        }
    }
}