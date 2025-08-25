using BSUIRScheduleDESK.ViewModels;
using System.Windows;
using System.Windows.Input;

namespace BSUIRScheduleDESK.Views;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        this.Owner = App.Current.MainWindow;
    }
    
    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
    private void ColorPicker_ColorChanged()
    {
        if(this.DataContext is ISettingsViewModel viewModel)
        {
            viewModel.UpdateColors();
        }
    }
}
