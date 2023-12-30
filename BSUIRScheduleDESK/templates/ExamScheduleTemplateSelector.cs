using BSUIRScheduleDESK.classes;
using System.Windows;
using System.Windows.Controls;

namespace BSUIRScheduleDESK.templates
{
    public class ExamScheduleTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement? element = container as FrameworkElement;
            if (element != null && item != null && item is Schedule)
            {
                Schedule? schedule = item as Schedule;
                if (schedule != null)
                {
                    if (schedule.announcement)
                    {
                        return element.FindResource("announcementTemplate") as DataTemplate;
                    }
                    else
                    {
                        return element.FindResource("scheduleTemplate") as DataTemplate;
                    }
                }
            }
            return element.FindResource("emptyTemplate") as DataTemplate;
        }

    }
}
