using BSUIRScheduleDESK.services;
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
    }
}
