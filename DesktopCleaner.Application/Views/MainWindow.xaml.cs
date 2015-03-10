using System.Windows;
using DesktopCleaner.Application.ViewModels;
using Microsoft.Practices.Unity;

namespace DesktopCleaner.Application.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [Dependency]
        public MainWindowViewModel ViewModel
        {
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
