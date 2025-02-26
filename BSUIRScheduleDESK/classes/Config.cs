using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

namespace BSUIRScheduleDESK.Classes
{
    public class Config : INotifyPropertyChanged
    {
        private static Config instance;
        private static ConfigService service = new ConfigService();
        static Config()
        {
            instance = service.Load();
        }
        public static Config Instance
        {
            get => instance;
        }

        private bool firstSubGroup = true;
        public bool FirstSubGroup
        {
            get => firstSubGroup;
            set
            {
                if (value == firstSubGroup) return;
                firstSubGroup = value;
                OnPropertyChanged();
            }
        }
        private bool secondSubGroup = true;
        public bool SecondSubGroup
        {
            get => secondSubGroup;
            set
            {
                if (value == secondSubGroup) return;
                secondSubGroup = value;
                OnPropertyChanged();
            }
        }
        private bool showExams = true;
        public bool ShowExams
        {
            get => showExams;
            set
            {
                if (value == showExams) return;
                showExams = value;
                OnPropertyChanged();
            }
        }
        private int currentWeek = 0;
        public int CurrentWeek
        {
            get => currentWeek;
            set
            {
                if (value == currentWeek) return;
                currentWeek = value;
                OnPropertyChanged();
            }
        }
        private DateTime lastStartup = DateTime.MinValue;
        public DateTime LastStartup
        {
            get => lastStartup;
            set
            {
                if (value == lastStartup) return;
                lastStartup = value;
                OnPropertyChanged();
            }
        }
        private string lectureColor = "#FF1E7145";
        public string LectureColor
        {
            get => lectureColor;
            set
            {
                if(value == lectureColor) return;
                lectureColor = value;
                OnPropertyChanged();
            }
        }
        private string practiceColor = "#FFFFC40D";
        public string PracticeColor
        {
            get => practiceColor;
            set
            {
                if(value == practiceColor) return;
                practiceColor = value; 
                OnPropertyChanged();
            }
        }
        private string labColor = "#FFB91D47";
        public string LabColor
        {
            get => labColor;
            set
            {
                if(value == labColor) return;
                labColor = value; 
                OnPropertyChanged();
            }
        }
        private string examColor = "#FF00ABA9";
        public string ExamColor
        {
            get => examColor;
            set
            {
                if(value == examColor) return;
                examColor = value;
                OnPropertyChanged();
            }
        }
        private string consultationColor = "#FF2B5797";
        public string ConsultationColor
        {
            get => consultationColor;
            set
            {
                if(value ==  consultationColor) return;
                consultationColor = value;
                OnPropertyChanged();
            }
        }
        private string unknownColor = "FF1D1D1D";
        public string UnknownColor
        {
            get => unknownColor;
            set
            {
                if(value == unknownColor) return;
                unknownColor = value; 
                OnPropertyChanged();
            }
        }
        private string favoriteSchedules = string.Empty;
        public string FavoriteSchedules
        {
            get => favoriteSchedules;
            set
            {
                if(value == favoriteSchedules) return;
                favoriteSchedules = value;
                OnPropertyChanged();
            }
        }
        private string currentTheme = "IISTheme";
        public string CurrentTheme
        {
            get => currentTheme;
            set
            {
                if(value == currentTheme) return;
                currentTheme = value;
                OnPropertyChanged();
            }
        }
        private string indexes = "[14,4,0,8,5,9]";
        public string Indexes
        {
            get => indexes;
            set
            {
                if(value == indexes) return;
                indexes = value;
                OnPropertyChanged();
            }
        }

        private bool showExpiredLessons = true;

        public bool ShowExpiredLessons
        {
            get => showExpiredLessons;
            set
            {
                if (value == showExpiredLessons) return;
                showExpiredLessons = value;
                OnPropertyChanged();
            }
        }

        private string currentLanguage = "ru";

        public string CurrentLanguage
        {
            get => currentLanguage;
            set
            {
                if(value == currentLanguage) return;
                currentLanguage = value;
                OnPropertyChanged();
            }
        }

        private bool showAllLessons = false;
        public bool ShowAllLessons
        {
            get => showAllLessons;
            set
            {
                if(value == showAllLessons) return;
                showAllLessons = value;
                OnPropertyChanged();
            }
        }

        public void Save()
        {
            service.Save(this);
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    internal class ConfigService
    {
        private const string PATH = @"\config.json";
        private string configPath;

        public ConfigService()
        {
            configPath = Directory.GetCurrentDirectory() + PATH;
            if(!File.Exists(configPath))
            {
                Save(new Config());
            }
        }
        public async Task<Config> LoadAsync()
        {
            Config? config = new Config();
            using(var fs = new FileStream(configPath, FileMode.Open, FileAccess.Read))
            {
                config = await JsonSerializer.DeserializeAsync<Config>(fs);
            }
            return config;
        }
        public Config Load()
        {
            string json = File.ReadAllText(configPath);
            Config? config = JsonSerializer.Deserialize<Config>(json);
            if(config == null)
                return new Config();
            else
                return config;
        }
        public async void SaveAsync(Config config)
        {
            using(var fs = new FileStream(configPath, FileMode.Create, FileAccess.Write))
            {
                await JsonSerializer.SerializeAsync(fs, config);
                await fs.DisposeAsync();
            }
        }
        public void Save(Config config)
        {
            string json = JsonSerializer.Serialize(config);
            File.WriteAllText(configPath, json);
        }
    }
}
