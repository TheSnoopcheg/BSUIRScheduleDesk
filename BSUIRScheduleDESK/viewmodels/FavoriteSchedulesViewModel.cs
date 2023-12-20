using BSUIRScheduleDESK.classes;
using BSUIRScheduleDESK.models;
using BSUIRScheduleDESK.services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace BSUIRScheduleDESK.viewmodels
{
    public class FavoriteSchedulesViewModel : Notifier
    {
        readonly ScheduleModel _model = new ScheduleModel();
        public FavoriteSchedulesViewModel()
        {
            EventService.ScheduleFavorited += AddSchedule;
        }
        private ICommand? openSchedule;
        public ICommand OpenSchedule
        {
            get
            {
                return openSchedule ??
                    (openSchedule = new RelayCommand(obj =>
                    {
                        if(obj is FavoriteSchedule v)
                        {
                            EventService.FavoriteScheduleSelected_Invoke(v);
                        }
                    }));
            }
        }

        private ICommand? deleteSchedule;
        public ICommand DeleteSchedule
        {
            get
            {
                return deleteSchedule ??
                    (deleteSchedule = new RelayCommand(obj =>
                    {
                        if(obj is FavoriteSchedule v)
                        {
                            _model.DeleteSchedule(v);
                            EventService.ScheduleUnFavorited_Invoke(v);
                        }
                    }));
            }
        }

        public void AddSchedule(GroupSchedule schedule, bool isProc)
        {
            _model.AddSchedule(schedule, isProc);
        }

        public ReadOnlyObservableCollection<FavoriteSchedule> FavoriteSchedules => _model.FavoriteSchedules;
    }
}
