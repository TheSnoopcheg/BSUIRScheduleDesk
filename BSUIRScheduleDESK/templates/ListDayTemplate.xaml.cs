using System.Windows;
using System.Windows.Controls;

namespace BSUIRScheduleDESK.templates
{
    /// <summary>
    /// Interaction logic for ListDaySchedule.xaml
    /// </summary>
    public partial class ListDayTemplate : UserControl
    {
        public ListDayTemplate()
        {
            InitializeComponent();
        }

        #region Date

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
            "Date",
            typeof(string),
            typeof(ListDayTemplate),
            new FrameworkPropertyMetadata(string.Empty));

        public string Date
        {
            get { return (string)GetValue(DateProperty);}
            set { SetValue(DateProperty, value); }
        }

        #endregion
    }
}
