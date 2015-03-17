using System;
using System.Drawing;
using System.Windows;
using DesktopCleaner.Application.ViewModels;
using Microsoft.Practices.Unity;

namespace DesktopCleaner.Application.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        [Dependency]
        public MainWindowViewModel ViewModel
        {
            set { DataContext = value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            SetupTrayIcon();
        }

        private void SetupTrayIcon()
        {
            var notifyIcon = new System.Windows.Forms.NotifyIcon
            {
                Icon = new Icon("Icon.ico"),
                Visible = true
            };
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                Hide();
            }

            base.OnStateChanged(e);
        }
    }
}
