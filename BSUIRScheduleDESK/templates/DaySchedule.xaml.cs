using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using System;
using System.Collections.ObjectModel;

namespace BSUIRScheduleDESK.templates
{
    public partial class DaySchedule : UserControl
    {
        public DaySchedule()
        {
            InitializeComponent();
        }

        #region LoadScheduleCommand

        public static readonly DependencyProperty LoadScheduleCommandProperty = DependencyProperty.Register(
            "LoadScheduleCommand",
            typeof(ICommand),
            typeof(DaySchedule),
            new FrameworkPropertyMetadata(null));

        public ICommand LoadScheduleCommand
        {
            get {  return (ICommand)GetValue(LoadScheduleCommandProperty);}
            set {  SetValue(LoadScheduleCommandProperty, value);}
        }

        #endregion

        #region Dates

        public static readonly DependencyProperty DatesProperty = DependencyProperty.Register(
            "Dates",
            typeof(ObservableCollection<DateTime>),
            typeof(DaySchedule),
            new FrameworkPropertyMetadata(null));

        public ObservableCollection<DateTime> Dates
        {
            get { return (ObservableCollection<DateTime>)GetValue(DatesProperty); }
            set {  SetValue(DatesProperty, value); }
        }

        #endregion
    }
}