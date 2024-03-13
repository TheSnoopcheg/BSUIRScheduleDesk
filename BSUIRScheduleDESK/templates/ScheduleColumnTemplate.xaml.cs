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
        public ScheduleColumnTemplate()
        {
            InitializeComponent(); 
            EventService.SubgroupUpdated += OnSub;
            EventService.WeekUpdated += OnSub;
        }
        public void OnSub()
        {
            if (classesPlates.ItemsSource != null)
                CollectionViewSource.GetDefaultView(classesPlates.ItemsSource).Refresh();
        }
        public void OnSub(int t)
        {
            if (classesPlates.ItemsSource != null)
                CollectionViewSource.GetDefaultView(classesPlates.ItemsSource).Refresh();
        }
    }
}
