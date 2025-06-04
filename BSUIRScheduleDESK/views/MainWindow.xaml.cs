using System;
using System.Windows;
using System.Windows.Input;

namespace BSUIRScheduleDESK.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.SourceInitialized += new EventHandler(MainWindow_SourceInitialized);
        }

        void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            WindowSizing.WindowInitialized(this);
        }

        private void Title_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.ChangedButton == MouseButton.Left)
            {
                if (e.ClickCount == 2)
                    AbjustWindowSize();
                else
                    DragMove();
            }
        }
        private void AbjustWindowSize()
        {
            WindowState = WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            AbjustWindowSize();
        }
    }
}