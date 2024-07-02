using BSUIRScheduleDESK.services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
#if DEBUG
using System.Diagnostics;
#endif

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
