using BSUIRScheduleDESK.classes;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BSUIRScheduleDESK.templates
{
    public class ExamScheduleTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement? element = container as FrameworkElement;
            if (element != null && item != null && item is IGrouping<string?, Schedule>)
            {
                IGrouping<string?, Schedule>? schedules = item as IGrouping<string?, Schedule>;
                if (schedules != null)
                {
                    return element.FindResource("scheduleTemplate") as DataTemplate;
                    //if (schedule.announcement)
                    //{
                    //    return element.FindResource("announcementTemplate") as DataTemplate;
                    //}
                    //else
                    //{
                    //}
                }
            }
            return element.FindResource("emptyTemplate") as DataTemplate;
        }

    }
}
