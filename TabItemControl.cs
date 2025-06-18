using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Wpf.Ui.Controls;

namespace WpfApp1
{
    public class TabItemControl : UserControl, IComponentConnector
    {
        public static readonly DependencyProperty TabTitleProperty = DependencyProperty.Register("TabTitle", typeof(string), typeof(TabItemControl), new PropertyMetadata("Tab"));
        public static readonly DependencyProperty IsSelectedProperty = DependencyProperty.Register("IsSelected", typeof(bool), typeof(TabItemControl), new PropertyMetadata(false, OnIsSelectedChanged));
        private bool _contentLoaded;

        public bool IsSelected
        {
            get => (bool)GetValue(IsSelectedProperty);
            set => SetValue(IsSelectedProperty, value);
        }

        public string TabTitle
        {
            get => (string)GetValue(TabTitleProperty);
            set => SetValue(TabTitleProperty, value);
        }

        public event RoutedEventHandler CloseTab;

        public TabItemControl()
        {
            InitializeComponent();
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TabItemControl tabItemControl)
            {
                tabItemControl.UpdateVisualState((bool)e.NewValue);
            }
        }

        private void UpdateVisualState(bool isSelected)
        {
            Opacity = isSelected ? 1.0 : 0.4;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to close this tab?", "Confirm Close", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                CloseTab?.Invoke(this, e);
            }
        }

        public void InitializeComponent()
        {
            if (_contentLoaded)
            {
                return;
            }
            _contentLoaded = true;
            Uri resourceLocator = new Uri("/Solara;component/tabitemcontrol.xaml", UriKind.Relative);
            Application.LoadComponent(this, resourceLocator);
        }

        void IComponentConnector.Connect(int connectionId, object target)
        {
            if (connectionId == 1)
            {
                ((Button)target).Click += CloseButton_Click;
            }
            else
            {
                _contentLoaded = true;
            }
        }
    }
}