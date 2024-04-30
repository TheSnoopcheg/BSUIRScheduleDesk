using BSUIRScheduleDESK.classes;
using System.Windows;
using System.Windows.Controls;
using BSUIRScheduleDESK.services;
using System.Collections.Generic;
using System;
#if DEBUG
using System.Diagnostics;
#endif

namespace BSUIRScheduleDESK.templates
{
    public class DayScheduleTemplateSelector : DataTemplateSelector
    {
        private List<bool> _subgroups;
        private bool _showExams;
        private int _week;
        public DayScheduleTemplateSelector()
        {
            EventService.SchedulePresentationUpdated += OnSchedulePresentationUpdate;
            EventService.WeekUpdated += OnWeekUpdate;
            _subgroups = new List<bool>(2)
            {
                Properties.Settings.Default.firstsubgroup,
                Properties.Settings.Default.secondsubgroup
            };
            _showExams = Properties.Settings.Default.showexams;
            _week = Properties.Settings.Default.currentweek;
            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                OnWeekUpdate(1);
        }
        private void OnWeekUpdate(int diff)
        {
            _week += diff;
            if (_week >= 5)
                _week -= 4;
            else if (_week <= 0)
                _week += 4;
        }
        private void OnSchedulePresentationUpdate()
        {
            _showExams = Properties.Settings.Default.showexams;
            _subgroups[0] = Properties.Settings.Default.firstsubgroup;
            _subgroups[1] = Properties.Settings.Default.secondsubgroup;
        }
        
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement? element = container as FrameworkElement;
            if(element != null && item != null && item is Schedule)
            {
                Schedule? schedule = item as Schedule;
                if(schedule != null)
                {
                    if (schedule.announcement)
                    {
                        return element.FindResource("announcementTemplate") as DataTemplate;
                    }
                    if (schedule.lessonTypeAbbrev != "ПЗ" && schedule.lessonTypeAbbrev != "ЛК" && schedule.lessonTypeAbbrev != "ЛР" && !_showExams)
                        return element.FindResource("clearTemplate") as DataTemplate;
                    if (schedule.weekNumber != null)
                    {
                        if (schedule!.weekNumber!.Contains(_week))
                        {
                            if (schedule.numSubgroup == 0 || (schedule.numSubgroup == 1 && _subgroups[0]) || (schedule.numSubgroup == 2 && _subgroups[1]))
                            {
                                if (_showExams && (schedule.lessonTypeAbbrev == "Экзамен" || schedule.lessonTypeAbbrev == "Консультация"))
                                    return element.FindResource("sessionItemTemplate") as DataTemplate;
                                return element.FindResource("scheduleTemplate") as DataTemplate;
                            }
                        }
                    }
                }
            }
            return element.FindResource("clearTemplate") as DataTemplate;
        }
    }
}
