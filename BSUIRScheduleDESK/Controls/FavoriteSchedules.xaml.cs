using BSUIRScheduleDESK.Classes;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BSUIRScheduleDESK.Controls;

/// <summary>
/// Логика взаимодействия для FavoriteGroups.xaml
/// </summary>
public partial class FavoriteSchedules : UserControl
{
    public FavoriteSchedules()
    {
        InitializeComponent();
    }

    #region LoadCommandProperty

    public static readonly DependencyProperty LoadCommandProperty = DependencyProperty.Register(
        "LoadCommand",
        typeof(ICommand),
        typeof(FavoriteSchedules));

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
        typeof(FavoriteSchedules));

    public ICommand DeleteCommand
    {
        get { return (ICommand)GetValue(DeleteCommandProperty); }
        set { SetValue(DeleteCommandProperty, value); }
    }

    #endregion

    #region FavoriteSchedulesProperty

    public static readonly DependencyProperty SchedulesProperty = DependencyProperty.Register(
        "Schedules",
        typeof(ObservableCollection<FavoriteSchedule>),
        typeof(FavoriteSchedules));

    public ObservableCollection<FavoriteSchedule> Schedules
    {
        get { return (ObservableCollection<FavoriteSchedule>)GetValue(SchedulesProperty); }
        set { SetValue(SchedulesProperty, value); }
    }

    #endregion
}
