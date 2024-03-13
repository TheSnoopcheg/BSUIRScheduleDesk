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
            EventService.SubgroupUpdated += OnSub;
            EventService.WeekUpdated += OnSub;
        }
        public void OnSub()
        {
            if (classesList.ItemsSource != null)
                CollectionViewSource.GetDefaultView(classesList.ItemsSource).Refresh();
        }
        public void OnSub(int t)
        {
            if (classesList.ItemsSource != null)
                CollectionViewSource.GetDefaultView(classesList.ItemsSource).Refresh();
        }

        #region LoadScheduleCommand

        public static readonly DependencyProperty LoadScheduleCommandProperty = DependencyProperty.Register(
            "LoadScheduleCommand",
            typeof(ICommand),
            typeof(ListDayTemplate),
            new FrameworkPropertyMetadata(null));

        public ICommand LoadScheduleCommand
        {
            get { return (ICommand)GetValue(LoadScheduleCommandProperty); }
            set { SetValue(LoadScheduleCommandProperty, value); }
        }

        #endregion

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
