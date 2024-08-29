using BSUIRScheduleDESK.models;
using BSUIRScheduleDESK.services;
using BSUIRScheduleDESK.viewmodels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace BSUIRScheduleDESK
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<App>();

                    services.AddSingleton<IMainWindowModel, MainWindowModel>();
                    services.AddSingleton<IScheduleSearchModel, ScheduleSearchModel>();
                    services.AddSingleton<IAnnouncementModel, AnnouncementModel>();
                    services.AddSingleton<INoteModel, NoteModel>();

                    services.AddSingleton<IMainWindowViewModel, MainWindowViewModel>();
                    services.AddSingleton<IScheduleSearchViewModel, ScheduleSearchViewModel>();
                    services.AddSingleton<INoteViewModel, NoteViewModel>();
                    services.AddSingleton<IAnnouncementViewModel, AnnouncementViewModel>();

                    services.AddSingleton<IDateService, DateService>();
                    services.AddSingleton<INetworkService, NetworkService>();
                    services.AddSingleton<IInternetService, InternetService>();

                    services.AddTransient<INoteService, NoteService>();
                    services.AddTransient<IFavoriteSchedulesService, FavoriteSchedulesService>();
                    services.AddTransient<IAnnouncementService, AnnouncementService>();
                    services.AddTransient<IScheduleService, ScheduleService>();
                })
                .Build();
            var app = host.Services.GetService<App>();
            app?.InitializeComponent();
            app?.Run();
        }
    }
}
