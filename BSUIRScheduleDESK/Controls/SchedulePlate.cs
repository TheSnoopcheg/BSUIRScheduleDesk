using BSUIRScheduleDESK.Classes;
using BSUIRScheduleDESK.Services;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BSUIRScheduleDESK.Controls
{
    public enum PlateState
    {
        Maximized,
        Normal
    }
    public class SchedulePlate : Control
    {
        private const int MAX_WEEK = 4;
        static SchedulePlate()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SchedulePlate), new FrameworkPropertyMetadata(typeof(SchedulePlate)));
        }
        public SchedulePlate() : base() { EventService.ColorsUpdated += EventService_ColorsUpdated; }


        #region ScheduleProperty

        public static readonly DependencyProperty ScheduleProperty = DependencyProperty.Register(
            "Schedule",
            typeof(Lesson),
            typeof(SchedulePlate),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnScheduleChanged)));

        private static void OnScheduleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SchedulePlate schedule) return;
            if(e.NewValue != null)
            {
                if(schedule._subjectInfo != null)
                {
                    schedule.SetUpColor();
                    schedule.SetUpPlate();
                }
            }
        }

        public Lesson Schedule
        {
            get { return (Lesson)GetValue(ScheduleProperty); }
            set { SetValue(ScheduleProperty, value); }
        }

        #endregion

        #region CommandProperty

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(SchedulePlate));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region StateProperty

        public static readonly DependencyProperty StateProperty = DependencyProperty.Register(
            "State",
            typeof(PlateState),
            typeof(SchedulePlate));

        public PlateState State
        {
            get { return (PlateState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _typeBorder = (Border)GetTemplateChild(PART_TypeBorder);
            _subjectInfo = (StackPanel)GetTemplateChild(PART_SubjectInfo);
            _subjectName = (TextBlock)GetTemplateChild(PART_SubjectName);
            _toolTipLabel = (TextBlock)GetTemplateChild(PART_ToolTipLabel);
            _abbrevLabel = (TextBlock)GetTemplateChild(PART_AbbrevLabel);
            _announcement = (StackPanel)GetTemplateChild(PART_Announcement);
            _announcementText = (TextBlock)GetTemplateChild(PART_AnnouncementText);
            _datesWeeksLabel = (TextBlock)GetTemplateChild(PART_DatesWeeksLabel);
            _boldDatesWeeksLabel = (TextBlock)GetTemplateChild(PART_BoldDatesWeeksLabel);
            _employeeGroups = (EmployeeGroups)GetTemplateChild(PART_EmployeeGroups);
            _noteLabel = (TextBlock)GetTemplateChild(PART_NoteLabel);
            _auditoriesLabel = (TextBlock)GetTemplateChild(PART_AuditoriesLabel);
            _subGroupsLabel = (TextBlock)GetTemplateChild(PART_SubgroupsLabel);
            _scheduleTime = (ScheduleTime)GetTemplateChild(PART_ScheduleTime);
            _timeLabel = (TextBlock)GetTemplateChild(PART_TimeLabel);
            _timePanel = (StackPanel)GetTemplateChild(PART_TimePanel);

            _employeeGroups.Command = Command;

            SetUpColor();
            SetUpPlate();
        }

        private void SetUpPlate()
        {
            if (Schedule == null) return;

            if (Schedule.announcement)
            {
                _subjectInfo.Visibility = Visibility.Collapsed;
                _announcement.Visibility = Visibility.Visible;
                if(_timeLabel != null)
                {
                    _timeLabel.Text = Schedule.startLessonTime;
                    _timePanel.Visibility = Visibility.Visible;
                }
            }
            else
            {
                _announcementText.Visibility = Visibility.Collapsed;
                _subjectName.Text = Schedule.subject;
                _toolTipLabel.Text = Schedule.subjectFullName;
                _abbrevLabel.Text = "(" + Schedule.lessonTypeAbbrev + ")";
            }

            if(Schedule.lessonTypeAbbrev == "Экзамен" ||  Schedule.lessonTypeAbbrev == "Консультация")
            {
                _datesWeeksLabel.Text = $"{Langs.Language.Date}: ";
                _boldDatesWeeksLabel.Text = Schedule.dateLesson;
                _noteLabel.Text = Schedule.note;
                if (Schedule.numSubgroup != 0)
                    _subGroupsLabel.Text = Schedule.numSubgroup.ToString() + " п.";
            }
            else if (Schedule.announcement)
            {
                _announcementText.Text = Schedule.note;
                _datesWeeksLabel.Text = $"{Langs.Language.Date}: ";
                _boldDatesWeeksLabel.Text = Schedule.startLessonDate;
            }
            else
            {
                _datesWeeksLabel.Text = $"{Langs.Language.Weeks}: ";
                if(Schedule.weekNumber != null)
                    if(Schedule.weekNumber.Count != MAX_WEEK)
                        _boldDatesWeeksLabel.Text = string.Join(", ", Schedule.weekNumber?.Select(i => i.ToString()) ?? Enumerable.Empty<string>());
                else
                {
                    _boldDatesWeeksLabel.Visibility = Visibility.Collapsed;
                    _datesWeeksLabel.Visibility = Visibility.Collapsed;
                }
                _noteLabel.Text = Schedule.note;
                if(Schedule.numSubgroup != 0)
                    _subGroupsLabel.Text = Schedule.numSubgroup.ToString() + $" {Langs.Language.SubgroupShort}.";
            }

            if(Schedule.auditories != null)
                _auditoriesLabel.Text = string.Join("\n", Schedule.auditories);

            if (Schedule.employees == null)
                _employeeGroups.DataContext = Schedule.studentGroups;
            else
                _employeeGroups.DataContext = Schedule.employees;

            if (_scheduleTime != null)
                if (TimeOnly.TryParse(Schedule.startLessonTime, out TimeOnly time))
                    _scheduleTime.StartTime = time;
        }

        private void SetUpColor()
        {
            if (_typeBorder == null) return;
            switch (Schedule.lessonTypeAbbrev)
            {
                case "ЛК":
                    _typeBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Config.Instance.LectureColor));
                    break;
                case "ПЗ":
                    _typeBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Config.Instance.PracticeColor));
                    break;
                case "ЛР":
                    _typeBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Config.Instance.LabColor));
                    break;
                case "Экзамен":
                    _typeBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Config.Instance.ExamColor));
                    break;
                case "Консультация":
                    _typeBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Config.Instance.ConsultationColor));
                    break;
                default:
                    _typeBorder.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Config.Instance.UnknownColor));
                    break;
            }
        }
        private void EventService_ColorsUpdated()
        {
            SetUpColor();
        }

        private const string PART_TypeBorder = "PART_TypeBorder";
        private const string PART_SubjectInfo = "PART_SubjectInfo";
        private const string PART_SubjectName = "PART_SubjectName";
        private const string PART_ToolTipLabel = "PART_ToolTipLabel";
        private const string PART_AbbrevLabel = "PART_AbbrevLabel";
        private const string PART_Announcement = "PART_Announcement";
        private const string PART_AnnouncementText = "PART_AnnouncementText";
        private const string PART_DatesWeeksLabel = "PART_DatesWeeksLabel";
        private const string PART_BoldDatesWeeksLabel = "PART_BoldDatesWeeksLabel";
        private const string PART_EmployeeGroups = "PART_EmployeeGroups";
        private const string PART_NoteLabel = "PART_NoteLabel";
        private const string PART_AuditoriesLabel = "PART_AuditoriesLabel";
        private const string PART_SubgroupsLabel = "PART_SubgroupsLabel";
        private const string PART_ScheduleTime = "PART_ScheduleTime";
        private const string PART_TimeLabel = "PART_TimeLabel";
        private const string PART_TimePanel = "PART_TimePanel";

        private Border _typeBorder;
        private StackPanel _subjectInfo;
        private TextBlock _subjectName;
        private TextBlock _toolTipLabel;
        private TextBlock _abbrevLabel;
        private StackPanel _announcement;
        private TextBlock _announcementText;
        private TextBlock _datesWeeksLabel;
        private TextBlock _boldDatesWeeksLabel;
        private EmployeeGroups _employeeGroups;
        private TextBlock _noteLabel;
        private TextBlock _auditoriesLabel;
        private TextBlock _subGroupsLabel;
        private ScheduleTime _scheduleTime;
        private TextBlock _timeLabel;
        private StackPanel _timePanel;
    }
}