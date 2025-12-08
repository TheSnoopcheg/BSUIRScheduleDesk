using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BSUIRScheduleDESK.Views;

public partial class EmployeeInfoWindow : Window
{
    public EmployeeInfoWindow()
    {
        InitializeComponent();
        this.Owner = App.Current.MainWindow;
    }
    private void OnCloseCalled()
    {
        this.DialogResult = true;
        this.Close();
    }
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        OnCloseCalled();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if(e.Key == Key.Escape)
            OnCloseCalled();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
    {
        if (sender is not Image image) return;
        image.Source = new BitmapImage(new Uri("/Assets/unknown-person-placeholder.png", UriKind.Relative));
    }
}
