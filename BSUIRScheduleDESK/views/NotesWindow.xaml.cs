using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace BSUIRScheduleDESK.Views;

/// <summary>
/// Interaction logic for NotesWindow.xaml
/// </summary>
public partial class NotesWindow : Window
{
    public NotesWindow()
    {
        InitializeComponent();
        this.Owner = App.Current.MainWindow;
        noteDatePicker.Language = XmlLanguage.GetLanguage(CultureInfo.CurrentUICulture.IetfLanguageTag);
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
        {
            DragMove();
        }
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        this.DialogResult = true;
        this.Close();
    }
}
