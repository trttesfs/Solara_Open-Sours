using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Wpf.Ui.Controls;

namespace WpfApp1
{
    public class ScriptHub : Page, IComponentConnector
    {
        private MainWindow _mainWindow;
        internal Grid MainGrid;
        internal TextBox SearchBar;
        internal WrapPanel ScriptPanel;
        private bool _contentLoaded;

        public ScriptHub(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
            InitializeComponent();
        }

        public void GetResults(string query)
        {
            ScriptHub.<GetResults>d__2 <GetResults>d__;
            <GetResults>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            <GetResults>d__.<>4__this = this;
            <GetResults>d__.query = query;
            <GetResults>d__.<>1__state = -1;
            <GetResults>d__.<>t__builder.Start<ScriptHub.<GetResults>d__2>(ref <GetResults>d__);
        }

        private void TextBox_Change(object sender, TextChangedEventArgs e)
        {
            GetResults(SearchBar.Text);
        }

        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            Uri resourceLocator = new Uri("/Solara;component/scripthub.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
        }

        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    MainGrid = (Grid)target;
                    break;
                case 2:
                    SearchBar = (TextBox)target;
                    SearchBar.TextChanged += TextBox_Change;
                    break;
                case 3:
                    ScriptPanel = (WrapPanel)target;
                    break;
                default:
                    _contentLoaded = true;
                    break;
            }
        }
    }
}