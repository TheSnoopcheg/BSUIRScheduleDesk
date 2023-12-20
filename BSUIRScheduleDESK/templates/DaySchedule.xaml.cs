using System.Windows.Controls;
using BSUIRScheduleDESK.services;
using System.Windows.Data;
using System.Windows;
using System.Windows.Input;

namespace BSUIRScheduleDESK.templates
{
    public partial class DaySchedule : UserControl
    {
        public DaySchedule()
        {
            InitializeComponent();
            EventService.SubgroupUpdated += OnSub;
            EventService.WeekUpdated += OnSub;
        }
        public void OnSub()
        {
            //if(sch.ItemsSource != null)
                //CollectionViewSource.GetDefaultView(sch.ItemsSource).Refresh();
        }
        public void OnSub(int t)
        {
            //if (sch.ItemsSource != null)
              //  CollectionViewSource.GetDefaultView(sch.ItemsSource).Refresh();
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
    }
}