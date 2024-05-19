using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace BSUIRScheduleDESK.views
{
    /// <summary>
    /// Interaction logic for ModalWindow.xaml
    /// </summary>

    public enum ModalWindowButtons{
        OK,
        OKCancel,
        YesNo,
        YesNoCancel
    }
    public partial class ModalWindow : Window
    {
        public ModalWindow() : this("Undefined")
        { }
        public ModalWindow(string? wTitle) : this(wTitle, "")
        { }
        public ModalWindow(string? wTitle, string? imageUrl) : this(wTitle, imageUrl, "")
        { }
        public ModalWindow(string? wTitle, string? imageUrl, string? wContent) : this(wTitle, imageUrl, wContent, ModalWindowButtons.OK) 
        { }
        public ModalWindow(string? wTitle, string? imageUrl, string? wContent, ModalWindowButtons buttons)
        {
            WTitle = wTitle;
            ImageUrl = imageUrl;
            WContent = wContent;
            Buttons = buttons;
            InitializeComponent();
            this.Owner = App.Current.MainWindow;
        }
        private ModalWindowButtons _buttons;
        public ModalWindowButtons Buttons
        {
            get => _buttons;
            private set => _buttons = value;
        }
        private string? _wTitle;
        public string? WTitle
        {
            get => _wTitle;
            private set => _wTitle = value;
        }

        private string? _imageUrl;
        public string? ImageUrl
        {
            get => _imageUrl;
            private set => _imageUrl = value;
        }

        private string? _wContent;
        public string? WContent
        {
            get => _wContent;
            private set => _wContent = value;
        }

        private void firstButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                this.DialogResult = false;
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void secondButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult= false;
        }
    }
}
