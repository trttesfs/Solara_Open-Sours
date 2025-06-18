using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Wpf.Ui.Controls;

namespace WpfApp1
{
    public class ScriptItem : UserControl, IComponentConnector
    {
        public string ScriptName;
        public string ScriptCode;
        internal Image ScriptImage;
        internal TextBlock ScriptTitle;
        internal Button ActionButton;
        private bool _contentLoaded;

        public ScriptItem(string name, string code)
        {
            ScriptName = name;
            ScriptCode = code;
            InitializeComponent();
            ScriptTitle.Text = name;
        }

        public Task InitializeAsync(string img)
        {
            ScriptItem.<InitializeAsync>d__3 <InitializeAsync>d__;
            <InitializeAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
            <InitializeAsync>d__.<>4__this = this;
            <InitializeAsync>d__.img = img;
            <InitializeAsync>d__.<>1__state = -1;
            <InitializeAsync>d__.<>t__builder.Start<ScriptItem.<InitializeAsync>d__3>(ref <InitializeAsync>d__);
            return <InitializeAsync>d__.<>t__builder.Task;
        }

        private Task LoadImageFromUrl(string url)
        {
            ScriptItem.<LoadImageFromUrl>d__4 <LoadImageFromUrl>d__;
            <LoadImageFromUrl>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
            <LoadImageFromUrl>d__.<>4__this = this;
            <LoadImageFromUrl>d__.url = url;
            <LoadImageFromUrl>d__.<>1__state = -1;
            <LoadImageFromUrl>d__.<>t__builder.Start<ScriptItem.<LoadImageFromUrl>d__4>(ref <LoadImageFromUrl>d__);
            return <LoadImageFromUrl>d__.<>t__builder.Task;
        }

        private void ActionButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow._home.AddTab(ScriptName, ScriptCode);
            mainWindow.MainFrame.Content = mainWindow._home;
        }

        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            Uri resourceLocator = new Uri("/Solara;component/scriptitem.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
        }

        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ScriptImage = (Image)target;
                    break;
                case 2:
                    ScriptTitle = (TextBlock)target;
                    break;
                case 3:
                    ActionButton = (Button)target;
                    ActionButton.Click += ActionButton_Click;
                    break;
                default:
                    _contentLoaded = true;
                    break;
            }
        }
    }
}