using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;
using Newtonsoft.Json;
using Wpf.Ui.Controls;

namespace WpfApp1
{
    public class MainWindow : FluentWindow, IComponentConnector
    {
        [DllImport("SolaraV3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int Start([MarshalAs(UnmanagedType.LPStr)] string guid);

        [DllImport("SolaraV3.dll", CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetStatus();

        private static string endpoint3 = "https://pastebin.com/raw/h8GnGH0Y";
        private static string endpoint = "https://getsolara.dev/api/endpoint.json";
        private static string endpoint2 = "https://api.getsolara.gg/api/endpoint.json";
        private static string json;
        private static SoftwareInfo softwareInfo;
        private static string GUID = Guid.NewGuid().ToString("N").Substring(0, 16);
        public Settings _settings;
        public Home _home;
        public ScriptHub _scriptHub;
        private Thread _checkThread;
        public WebClient client = new WebClient();
        private static string robloxProcessName = "RobloxPlayerBeta";
        private static bool stopChecker = false;
        public const string DllName = "SolaraV3.dll";
        private int port;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationTokenSource cancellationTokenSource2;
        internal MainWindow fluentWindow;
        internal Grid PageLayout;
        internal Frame MainFrame;
        internal Ellipse Status;
        internal Button MinimizeBtn;
        internal Wpf.Ui.Controls.Image MaximizeImg;
        private bool _contentLoaded;

        public MainWindow()
        {
            _scriptHub = new ScriptHub(this);
            _settings = new Settings(this);
            _home = new Home(this, GUID);
            try
            {
                StartupCheck();
            }
            catch
            {
            }
            StartSocket(GUID);
            softwareInfo = JsonConvert.DeserializeObject<SoftwareInfo>(json);
            InitializeComponent();
            CheckRobloxProcess();
            Loaded += MainWindow_Loaded;
            Closing += ClosingEvent;
            MainFrame.Content = _home;
        }

        private void ClosingEvent(object sender, CancelEventArgs e)
        {
            MainWindow.<ClosingEvent>d__20 <ClosingEvent>d__;
            <ClosingEvent>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            <ClosingEvent>d__.<>4__this = this;
            <ClosingEvent>d__.e = e;
            <ClosingEvent>d__.<>1__state = -1;
            <ClosingEvent>d__.<>t__builder.Start<MainWindow.<ClosingEvent>d__20>(ref <ClosingEvent>d__);
        }

        private static string ComputeSHA256(string filePath)
        {
            using FileStream fileStream = File.OpenRead(filePath);
            using SHA256 sha = SHA256.Create();
            return BitConverter.ToString(sha.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant();
        }

        private void CheckRobloxProcess()
        {
            MainWindow.<CheckRobloxProcess>d__22 <CheckRobloxProcess>d__;
            <CheckRobloxProcess>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
            <CheckRobloxProcess>d__.<>4__this = this;
            <CheckRobloxProcess>d__.<>1__state = -1;
            <CheckRobloxProcess>d__.<>t__builder.Start<MainWindow.<CheckRobloxProcess>d__22>(ref <CheckRobloxProcess>d__);
        }

        private void UpdateStatus(string color)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Status.Fill = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
            });
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            client.Headers.Add("Content-Type", "plain/text");
            client.Headers.Add("User-Agent", "Solara/v3.0");
            client.Headers.Add("GUID", GUID);
        }

        private void CheckLoop()
        {
            while (true)
            {
                try
                {
                    string status = GetStatus().ToString();
                    if (status == "100")
                    {
                        UpdateStatus("#FF9FE2BF");
                    }
                    else if (status == "0")
                    {
                        UpdateStatus("#FFFF7F7F");
                    }
                    else
                    {
                        UpdateStatus("#FFF5BC42");
                    }
                }
                catch
                {
                }
                Thread.Sleep(500);
            }
        }

        private void StartSocket(string guid)
        {
            port = Start(guid);
            _checkThread = new Thread(CheckLoop)
            {
                IsBackground = true
            };
            _checkThread.Start();
        }

        private static Process GetRobloxProcess()
        {
            Process[] processes = Process.GetProcessesByName(robloxProcessName);
            if (processes.Length != 0)
            {
                return processes[0];
            }
            processes = Process.GetProcessesByName("eurotruck2");
            if (processes.Length != 0)
            {
                return processes[0];
            }
            processes = Process.GetProcessesByName("eurotrucks2");
            if (processes.Length != 0)
            {
                return processes[0];
            }
            return null;
        }

        private static void LaunchRobloxWithArgs(string exePath)
        {
            try
            {
                using RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software\\ROBLOX Corporation\\Environments\\RobloxPlayer\\Channel", true);
                if (registryKey != null)
                {
                    registryKey.SetValue("www.roblox.com", string.Empty);
                }
            }
            catch
            {
            }
            string arguments = $"--app -isInstallerLaunch --launchtime={DateTimeOffset.Now.ToUnixTimeMilliseconds()}";
            Process.Start(exePath, arguments).WaitForExit();
        }

        private void StartupCheck()
        {
            try
            {
                json = client.DownloadString(endpoint);
            }
            catch
            {
                try
                {
                    json = client.DownloadString(endpoint2);
                }
                catch
                {
                    json = client.DownloadString(endpoint3);
                }
            }
            softwareInfo = JsonConvert.DeserializeObject<SoftwareInfo>(json);
            RobloxInfo robloxInfo = JsonConvert.DeserializeObject<RobloxInfo>(client.DownloadString(softwareInfo.VersionUrl));
            if (softwareInfo.SupportedClient != robloxInfo.clientVersionUpload)
            {
                System.Windows.Forms.MessageBox.Show("Solara is patched. Please wait for an update.", "Solara", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, (System.Windows.Forms.MessageBoxOptions)262144);
            }
        }

        private void fluentWindow_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void HomeButtonClicked(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content != _home)
            {
                MainFrame.Content = _home;
            }
        }

        private void SettingsButtonClicked(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content != _settings)
            {
                MainFrame.Content = _settings;
            }
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        private void MaximieClicked(object sender, RoutedEventArgs e)
        {
            BitmapImage bitmapImage = new BitmapImage();
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("pack://application:,,,/Images/compress_icon.png");
                bitmapImage.EndInit();
                MaximizeImg.Source = bitmapImage;
            }
            else
            {
                WindowState = WindowState.Normal;
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri("pack://application:,,,/Images/maximize_icon.png");
                bitmapImage.EndInit();
                MaximizeImg.Source = bitmapImage;
            }
        }

        private void CloseClicked(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MinimizeClicked(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ScriptHubClicked(object sender, RoutedEventArgs e)
        {
            if (MainFrame.Content != _scriptHub)
            {
                MainFrame.Content = _scriptHub;
            }
        }

        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            Uri resourceLocator = new Uri("/Solara;component/mainwindow.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
        }

        void IComponentConnector.Connect(int connectionId, object target)
        {
            switch (connectionId)
            {
                case 1:
                    fluentWindow = (MainWindow)target;
                    break;
                case 2:
                    ((Grid)target).Loaded += fluentWindow_Loaded;
                    break;
                case 3:
                    PageLayout = (Grid)target;
                    break;
                case 4:
                    ((Button)target).Click += HomeButtonClicked;
                    break;
                case 5:
                    ((Button)target).Click += ScriptHubClicked;
                    break;
                case 6:
                    ((Button)target).Click += SettingsButtonClicked;
                    break;
                case 7:
                    MainFrame = (Frame)target;
                    break;
                case 8:
                    ((Grid)target).MouseDown += Grid_MouseDown;
                    break;
                case 9:
                    Status = (Ellipse)target;
                    break;
                case 10:
                    MinimizeBtn = (Button)target;
                    MinimizeBtn.Click += MaximieClicked;
                    break;
                case 11:
                    MaximizeImg = (Wpf.Ui.Controls.Image)target;
                    break;
                case 12:
                    ((Button)target).Click += CloseClicked;
                    break;
                case 13:
                    ((Button)target).Click += MinimizeClicked;
                    break;
                default:
                    _contentLoaded = true;
                    break;
            }
        }
    }
}