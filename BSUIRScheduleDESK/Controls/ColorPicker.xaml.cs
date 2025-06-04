using System;
using System.Windows;
using System.Windows.Controls;

namespace BSUIRScheduleDESK.Controls
{
    public partial class ColorPicker : UserControl
    {
        public ColorPicker()
        {
            InitializeComponent();
        }

        #region Text

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        #endregion

        #region Color

        public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
            "Color",
            typeof(string),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string Color
        {
            get { return (string)GetValue(ColorProperty); }
            set { SetValue(ColorProperty, value); }
        }

        #endregion

        #region SelectedColorIndex

        public static readonly DependencyProperty SelectedColorIndexProperty = DependencyProperty.Register(
            "SelectedColorIndex",
            typeof(int),
            typeof(ColorPicker),
            new FrameworkPropertyMetadata(0,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int SelectedColorIndex
        {
            get { return (int)GetValue(SelectedColorIndexProperty);}
            set { SetValue(SelectedColorIndexProperty, value);}
        }

        #endregion

        public event Action? ColorChanged;

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ColorChanged?.Invoke();
        }
    }
}
