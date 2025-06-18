using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Wpf.Ui.Controls;

namespace WpfApp1
{
    public class Settings : Page, IComponentConnector
    {
        private MainWindow _mainWindow;
        internal Settings fluentPage;
        internal ToggleSwitch TopMostSwitch;
        private bool _contentLoaded;

        public Settings(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
            TopMostSwitch.Checked += (s, e) => Application.Current.MainWindow.Topmost = true;
            TopMostSwitch.Unchecked += (s, e) => Application.Current.MainWindow.Topmost = false;
        }

        private void BackButtonClicked(object sender, RoutedEventArgs e)
        {
            if (_mainWindow != null && _mainWindow._home != null)
            {
                _mainWindow.MainFrame.Content = _mainWindow._home;
            }
            else
            {
                MessageBox.Show("Navigation failed: MainWindow or Home is not properly initialized.");
            }
        }

        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            Uri resourceLocator = new Uri("/Solara;component/settings.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
        }

        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                fluentPage = (Settings)target;
            }
            else if (connectionId == 2)
            {
                TopMostSwitch = (ToggleSwitch)target;
            }
            else
            {
                _contentLoaded = true;
            }
        }
    }
}