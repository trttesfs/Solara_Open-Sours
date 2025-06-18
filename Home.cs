using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Wpf.Ui.Controls;

namespace WpfApp1
{
    public class Home : Page, IComponentConnector
    {
        private readonly Button _addTabButton;
        private readonly MainWindow _mainWindow;
        private readonly Dictionary<TabItemControl, WebView2> _tabWebViewMap = new Dictionary<TabItemControl, WebView2>();
        private readonly Grid _grid;
        private readonly StackPanel _tabContainer;
        private WebView2 _currentTab;
        private bool _contentLoaded;

        internal Grid MainGrid;
        internal StackPanel TabContainer;
        internal Button AddTabBtn;

        public Home(MainWindow mainWindow, string guids)
        {
            InitializeComponent();
            _addTabButton = AddTabBtn;
            _tabContainer = TabContainer;
            _grid = MainGrid;
            _mainWindow = mainWindow;

            
            string htmlHeader = "<!DOCTYPE html>\n<html lang=\"en\" xmlns=\"http://www.w3.org/1999/xhtml\">\n<head>\n<script>\nconst guid = '';\n</script>\n";
            string monacoHtml = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Monaco\\index.html"));
            File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Monaco\\combined.html"), htmlHeader + monacoHtml);

           
            string solaraTabPath = @"C:\SolaraTab";
            if (!Directory.Exists(solaraTabPath))
            {
                Directory.CreateDirectory(solaraTabPath);
                AddTab("Tab 1", "");
            }
            else
            {
                foreach (string filePath in Directory.GetFiles(solaraTabPath))
                {
                    string tabName = Path.GetFileNameWithoutExtension(filePath);
                    string content = File.ReadAllText(filePath);
                    AddTab(tabName, content);
                }
            }
        }

        public void AddTab(string tabName, string content = "")
        {
           
            File.WriteAllText(Path.Combine(@"C:\SolaraTab", $"{tabName}.lua"), content);

            // Создание новой вкладки с WebView2
            var webView = new WebView2
            {
                Margin = new Thickness(0, 0, 0, 48),
                Visibility = Visibility.Collapsed,
                DefaultBackgroundColor = Color.Transparent
            };
            webView.Source = new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Monaco\\combined.html"));

            // Обработка завершения навигации WebView2
            webView.NavigationCompleted += async (sender, args) =>
            {
                
            };

            _grid.Children.Add(webView);
            Grid.SetRow(webView, 1);

            // Создание элемента вкладки
            var newTab = new TabItemControl { TabTitle = tabName };
            newTab.CloseTab += (s, e) => DeleteTab(newTab);
            newTab.MouseDown += (s, e) => SelectTab(newTab);

            _tabContainer.Children.Add(newTab);
            _tabWebViewMap[newTab] = webView;
            SelectTab(newTab);
            UpdateAddTabButtonPosition();
        }

        public async Task SaveTabs()
        {
          foreach (WebView2 webView in this._tabWebViewMap.Values)
			{
				webView.Visibility = Visibility.Collapsed;
			}
			if (this._tabWebViewMap.ContainsKey(selectedTab))
			{
				this._tabWebViewMap[selectedTab].Visibility = Visibility.Visible;
				this.currentTab = this._tabWebViewMap[selectedTab];
			}
			foreach (object obj in this._tabContainer.Children)
			{
				TabItemControl tabItemControl = obj as TabItemControl;
				if (tabItemControl != null)
				{
					tabItemControl.IsSelected = (tabItemControl == selectedTab);
				}
			}
        }

        public void SelectTab(TabItemControl selectedTab)
        {
            foreach (WebView2 webView in _tabWebViewMap.Values)
            {
                webView.Visibility = Visibility.Collapsed;
            }

            if (_tabWebViewMap.ContainsKey(selectedTab))
            {
                _tabWebViewMap[selectedTab].Visibility = Visibility.Visible;
                _currentTab = _tabWebViewMap[selectedTab];
            }

            foreach (object child in _tabContainer.Children)
            {
                if (child is TabItemControl tabItem)
                {
                    tabItem.IsSelected = (tabItem == selectedTab);
                }
            }
        }

        public void DeleteTab(TabItemControl tabToDelete)
        {
            if (_tabWebViewMap.ContainsKey(tabToDelete))
            {
                WebView2 webView = _tabWebViewMap[tabToDelete];
                _grid.Children.Remove(webView);
                _tabWebViewMap.Remove(tabToDelete);

                string filePath = Path.Combine(@"C:\SolaraTab", $"{tabToDelete.TabTitle}.lua");
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                int index = _tabContainer.Children.IndexOf(tabToDelete);
                _tabContainer.Children.Remove(tabToDelete);

                if (_tabContainer.Children.Count > 1)
                {
                    int newIndex = index < _tabContainer.Children.Count ? index : index - 1;
                    if (_tabContainer.Children[newIndex] is Button)
                    {
                        newIndex--;
                    }
                    if (newIndex >= 0 && newIndex < _tabContainer.Children.Count)
                    {
                        SelectTab((TabItemControl)_tabContainer.Children[newIndex]);
                    }
                }
            }
        }

        private void HomeLoaded(object sender, RoutedEventArgs e)
        {
            string address = $"http://localhost:9912/request";
            _mainWindow.client.UploadString(address, "POST", "initial_ping");
        }

        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            	Home.<ClearButton_Click>d__12 <ClearButton_Click>d__;
			<ClearButton_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ClearButton_Click>d__.<>4__this = this;
			<ClearButton_Click>d__.<>1__state = -1;
			<ClearButton_Click>d__.<>t__builder.Start<Home.<ClearButton_Click>d__12>(ref <ClearButton_Click>d__);
        }

        private void InjectButton_MouseClick1(object sender, RoutedEventArgs e)
        {
            _mainWindow.client.Encoding = Encoding.UTF8;
            string address = $"http://localhost:9912/request";
            string response = _mainWindow.client.UploadString(address, "POST", "Attach");

            switch (response)
            {
                case ".":
                    MessageBox.Show("Already attached.");
                    break;
                case ";":
                    MessageBox.Show("Open RobloxPlayerBeta.exe");
                    break;
                case "-":
                    MessageBox.Show("Failed to attach.");
                    break;
                case ">":
                    MessageBox.Show("Roblox version mismatch!");
                    break;
                case "]":
                    break;
                default:
                    _mainWindow.Status.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString("#FF9FE2BF");
                    break;
            }
        }

        private async void ExecuteClicked(object sender, RoutedEventArgs e)
        {
            Home.<ExecuteClicked>d__14 <ExecuteClicked>d__;
			<ExecuteClicked>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ExecuteClicked>d__.<>4__this = this;
			<ExecuteClicked>d__.<>1__state = -1;
			<ExecuteClicked>d__.<>t__builder.Start<Home.<ExecuteClicked>d__14>(ref <ExecuteClicked>d__);
        }

        private static string GetBootstrapperPath()
        {
            string[] args = Environment.GetCommandLineArgs();
            for (int i = 0; i < args.Length - 1; i++)
            {
                if (args[i] == "--bootstrapperPath")
                {
                    return args[i + 1];
                }
            }
            return string.Empty;
        }

        private async void OpenFile(object sender, RoutedEventArgs e)
        {
           Home.<OpenFile>d__16 <OpenFile>d__;
			<OpenFile>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<OpenFile>d__.<>4__this = this;
			<OpenFile>d__.<>1__state = -1;
			<OpenFile>d__.<>t__builder.Start<Home.<OpenFile>d__16>(ref <OpenFile>d__);
        }

        private async void SaveFile(object sender, RoutedEventArgs e)
        {
            Home.<SaveFile>d__17 <SaveFile>d__;
			<SaveFile>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<SaveFile>d__.<>4__this = this;
			<SaveFile>d__.<>1__state = -1;
			<SaveFile>d__.<>t__builder.Start<Home.<SaveFile>d__17>(ref <SaveFile>d__);
        }

        private void UpdateAddTabButtonPosition()
        {
            _tabContainer.Children.Remove(_addTabButton);
            _tabContainer.Children.Add(_addTabButton);
        }

        private void AddTabButton_Click(object sender, RoutedEventArgs e)
        {
            string tabName = $"Tab {_tabContainer.Children.Count}";
            AddTab(tabName, "");
            UpdateAddTabButtonPosition();
        }

        public void InitializeComponent()
        {
            if (_contentLoaded)
                return;

            _contentLoaded = true;
            Uri resourceLocator = new Uri("/Solara;component/home.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
        }

        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    ((Home)target).Loaded += HomeLoaded;
                    break;
                case 2:
                    MainGrid = (Grid)target;
                    break;
                case 3:
                    TabContainer = (StackPanel)target;
                    break;
                case 4:
                    AddTabBtn = (Button)target;
                    AddTabBtn.Click += AddTabButton_Click;
                    break;
                case 5:
                    ((Button)target).Click += ExecuteClicked;
                    break;
                case 6:
                    ((Button)target).Click += ClearButton_Click;
                    break;
                case 7:
                    ((Button)target).Click += InjectButton_MouseClick1;
                    break;
                case 8:
                    ((Button)target).Click += OpenFile;
                    break;
                case 9:
                    ((Button)target).Click += SaveFile;
                    break;
                default:
                    _contentLoaded = true;
                    break;
            }
        }
    }
}