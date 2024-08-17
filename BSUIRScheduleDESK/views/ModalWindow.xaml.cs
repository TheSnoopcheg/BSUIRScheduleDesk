using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace BSUIRScheduleDESK.views
{
    public enum ModalWindowButtons
    {
        OK,
        OKCancel,
        YesNo,
        YesNoCancel
    }
    public enum ModalWindowResult
    {
        OK,
        Canceled,
        Yes,
        No
    }
    /// <summary>
    /// Interaction logic for ModalWindow.xaml
    /// </summary>=
    public partial class ModalWindow : Window
    {
        private static ModalWindowResult result = ModalWindowResult.OK;

        private Button OK
        {
            get
            {
                var b = GetStyledButton();
                b.Content = "ОК";
                b.Click += delegate { result = ModalWindowResult.OK; Close(); };
                return b;
            }
        }
        private Button Cancel
        {
            get
            {
                var b = GetStyledButton();
                b.Content = "Отмена";
                b.Click += delegate { result = ModalWindowResult.Canceled; Close(); };
                return b;
            }
        }
        private Button Yes
        {
            get
            {
                var b = GetStyledButton();
                b.Content = "Да";
                b.Click += delegate { result = ModalWindowResult.Yes; Close(); };
                return b;
            }
        }
        private Button No
        {
            get
            {
                var b = GetStyledButton();
                b.Content = "Нет";
                b.Click += delegate { result = ModalWindowResult.No; Close(); };
                return b;
            }
        }

        private ModalWindow(string wContent = "", string wTitle = "", string wImageUrl = "", ModalWindowButtons wButtons=ModalWindowButtons.OK)
        {
            InitializeComponent();
            
            MakeUpWindow(wContent, wTitle, wImageUrl, wButtons);

            if(this != App.Current.MainWindow)
                this.Owner = App.Current.MainWindow;
        }

        public static ModalWindowResult Show(string content)
        {
            new ModalWindow(content).ShowDialog();
            return result;
        }
        public static ModalWindowResult Show(string content, string title)
        {
            new ModalWindow(content, title).ShowDialog();
            return result;
        }
        public static ModalWindowResult Show(string content, string title, string imageUrl)
        {
            new ModalWindow(content, title, imageUrl).ShowDialog();
            return result;
        }
        public static ModalWindowResult Show(string content, string title, string imageUrl, ModalWindowButtons buttons)
        {
            new ModalWindow(content, title, imageUrl, buttons).ShowDialog();
            return result;
        }

        private void MakeUpWindow(string content, string title, string imageUrl, ModalWindowButtons buttons)
        {
            TitleBlock.Text = title;
            ContentBlock.Text = content;

            ImageBlock.Source = GetImageSource(imageUrl);
            switch(buttons)
            {
                case ModalWindowButtons.OK:
                    ButtonsPanel.Children.Add(OK);
                    break;
                case ModalWindowButtons.OKCancel:
                    ButtonsPanel.Children.Add(OK);
                    ButtonsPanel.Children.Add(Cancel);
                    break;
                case ModalWindowButtons.YesNo:
                    ButtonsPanel.Children.Add(Yes);
                    ButtonsPanel.Children.Add(No);
                    break;
                case ModalWindowButtons.YesNoCancel:
                    ButtonsPanel.Children.Add(Yes);
                    ButtonsPanel.Children.Add(No);
                    ButtonsPanel.Children.Add(Cancel);
                    break;
                default:
                    ButtonsPanel.Children.Add(OK);
                    break;
            }
        }

        private BitmapSource GetImageSource(string imageUrl)
        {
            try
            {
                return new BitmapImage(new System.Uri(imageUrl));
            }
            catch
            {
                return null;
            }
        }

        private Button GetStyledButton()
        {
            Button b = new Button();
            try
            {
                Style style = FindResource("ModalWindowButtonStyle") as Style;
                if(style != null) 
                    b.Style = style;
            }
            catch { }
            return b;
        }

        private void Border_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                result = ModalWindowResult.Canceled;
                Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            result = ModalWindowResult.Canceled;
            Close();
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}
