using BSUIRScheduleDESK.classes;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BSUIRScheduleDESK.templates
{
    /// <summary>
    /// Логика взаимодействия для FavoriteGroups.xaml
    /// </summary>
    public partial class FavoriteGroups : UserControl
    {
        public FavoriteGroups()
        {
            InitializeComponent();
        }

        #region LoadCommandProperty

        public static readonly DependencyProperty LoadCommandProperty = DependencyProperty.Register(
            "LoadCommand",
            typeof(ICommand),
            typeof(FavoriteGroups));

        public ICommand LoadCommand
        {
            get { return  (ICommand)GetValue(LoadCommandProperty);}
            set { SetValue(LoadCommandProperty, value); }
        }

        #endregion

        #region DeleteCommandProperty

        public static readonly DependencyProperty DeleteCommandProperty = DependencyProperty.Register(
            "DeleteCommand",
            typeof(ICommand),
            typeof(FavoriteGroups));

        public ICommand DeleteCommand
        {
            get { return (ICommand)GetValue(DeleteCommandProperty); }
            set { SetValue(DeleteCommandProperty, value); }
        }

        #endregion

        #region

        public static readonly DependencyProperty SchedulesProperty = DependencyProperty.Register(
            "Schedules",
            typeof(ObservableCollection<FavoriteSchedule>),
            typeof(FavoriteGroups));

        public ObservableCollection<FavoriteSchedule> Schedules
        {
            get { return (ObservableCollection<FavoriteSchedule>)GetValue(SchedulesProperty); }
            set { SetValue(SchedulesProperty, value); }
        }

        #endregion
    }
}
