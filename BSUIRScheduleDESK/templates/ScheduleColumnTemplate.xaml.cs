using BSUIRScheduleDESK.services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BSUIRScheduleDESK.templates
{
    /// <summary>
    /// Interaction logic for ScheduleColumnTemplate.xaml
    /// </summary>
    public partial class ScheduleColumnTemplate : UserControl
    {
        public enum LocationEnum
        {
            First,
            Middle,
            Last
        }
        public ScheduleColumnTemplate()
        {
            InitializeComponent(); 
            EventService.SubgroupUpdated += OnSub;
            EventService.WeekUpdated += OnSub;
        }
        public void OnSub()
        {
            if (sch.ItemsSource != null)
                CollectionViewSource.GetDefaultView(sch.ItemsSource).Refresh();
        }
        public void OnSub(int t)
        {
            if (sch.ItemsSource != null)
                CollectionViewSource.GetDefaultView(sch.ItemsSource).Refresh();
        }
        #region Date

        public static readonly DependencyProperty DateProperty = DependencyProperty.Register(
            "Date",
            typeof(string),
            typeof(ScheduleColumnTemplate),
            new FrameworkPropertyMetadata(null));

        public string Date
        {
            get { return (string)GetValue(DateProperty);}
            set { SetValue(DateProperty, value); }
        }

        #endregion

        #region Location

        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register(
            "Location",
            typeof(LocationEnum),
            typeof(ScheduleColumnTemplate),
            new FrameworkPropertyMetadata(LocationEnum.Middle));

        public LocationEnum Location
        {
            get { return (LocationEnum)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        #endregion
    }
}
