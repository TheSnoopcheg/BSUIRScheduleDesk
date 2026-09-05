using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BSUIRScheduleDESK.Controls;

/// <summary>
/// Interaction logic for EmployeeGroups.xaml
/// </summary>
public partial class EmployeeGroups : UserControl
{
    private static readonly BitmapImage PlaceholderImage = CreatePlaceholderImage();
    private static BitmapImage CreatePlaceholderImage()
    {
        var image = new BitmapImage();
        image.BeginInit();
        image.UriSource = new Uri("/Assets/unknown-person-placeholder.png", UriKind.Relative);
        image.EndInit();
        image.Freeze();
        return image;
    }
    public EmployeeGroups()
    {
        InitializeComponent();
    }

    #region Command

    public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
        "Command",
        typeof(ICommand),
        typeof(EmployeeGroups),
        new FrameworkPropertyMetadata(null));

    public ICommand Command
    {
        get { return (ICommand)GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }
    #endregion

    private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
    {
        if (sender is not Image image) return;
        image.SetCurrentValue(Image.SourceProperty, PlaceholderImage);
    }
}
