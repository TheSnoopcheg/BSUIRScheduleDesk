using BSUIRScheduleDESK.Classes;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Controls.Primitives;
using System.Globalization;

namespace BSUIRScheduleDESK.Controls
{
    public class SchedulePresenter : Control
    {
        private int _minRow = -1;
        private int _maxRow = -1;
        private const int GRID_ROWS = 9;
        private const int GRID_ROWS_NORMAL = 12;
        private const int GRID_COLS = 7;
        private int _currentWeek = 0;
        private int _weekDiff = 0;

        private List<DateTime> _dates = new List<DateTime>();

        private Dictionary<TimeOnly, int> StartLessonDict = new Dictionary<TimeOnly, int>()
        {
            { TimeOnly.Parse("9:00"), 0},
            { TimeOnly.Parse("10:35"), 1},
            { TimeOnly.Parse("12:25"), 2},
            { TimeOnly.Parse("14:00"), 3},
            { TimeOnly.Parse("15:50"), 4},
            { TimeOnly.Parse("17:25"), 5}, 
            { TimeOnly.Parse("19:00"), 6},
            { TimeOnly.Parse("20:40"), 7}
        };
        static SchedulePresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SchedulePresenter), new FrameworkPropertyMetadata(typeof(SchedulePresenter)));
        }

        public SchedulePresenter() : base() 
        {
            _dates = GetCurrentWeekDates();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _scheduleView = (Grid)GetTemplateChild(PART_ScheduleView);
            _maximizedView = (Grid)GetTemplateChild(PART_MaximizedView);
            _normalView = (Grid)GetTemplateChild(PART_NormalScheduleView);
            _previousButton = (Button)GetTemplateChild(PART_PreviousWeekButton);
            _nextButton = (Button) GetTemplateChild(PART_NextWeekButton);
            _calendarButton = (Button)GetTemplateChild(PART_CalendarButton);
            _returnButton = (Button)GetTemplateChild(PART_ReturnButton);
            _firstSubGroup = (CheckBox)GetTemplateChild(PART_FirstSubGroup);
            _secondSubGroup = (CheckBox)GetTemplateChild(PART_SecondSubGroup);
            _showExams = (CheckBox)GetTemplateChild(PART_ShowExams);
            _currentWeekLabel = (TextBlock)GetTemplateChild(PART_CurrentWeekLabel);
            _calendarPopup = (Popup)GetTemplateChild(PART_CalendarPopup);
            _calendar = (System.Windows.Controls.Calendar)GetTemplateChild(PART_Calendar);
            _openDateButton = (Button)GetTemplateChild(PART_OpenDateButton);
            _todayBorder = (Border)GetTemplateChild(PART_TodayBorder);
            _emptyMessage = (TextBlock)GetTemplateChild(PART_EmptyMessage);

            _previousButton.Click += PreviousButton_Click;
            _nextButton.Click += NextButton_Click;
            _calendarButton.Click += CalendarButton_Click;
            _returnButton.Click += ReturnButton_Click;
            _firstSubGroup.Click += FirstSubGroup_Click;
            _secondSubGroup.Click += SecondSubGroup_Click;
            _showExams.Click += ShowExams_Click;
            _calendar.GotMouseCapture += Calendar_GotMouseCapture;
            _openDateButton.Click += OpenDateButton_Click;

            _currentWeekLabel.Text = "Неделя: " + CurrentWeek.ToString();
            _currentWeek = CurrentWeek;
            _firstSubGroup.IsChecked = FirstSubGroup;
            _secondSubGroup.IsChecked = SecondSubGroup;
            _showExams.IsChecked = ShowExams;

            SetUpMaximizedGrid();
            SetUpNormalGrid();
            SetUp();
            SetUpDatesMaximized();

            if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday)
                ChangeWeek(1);
        }

        #region LessonsProperty

        public static readonly DependencyProperty LessonsProperty = DependencyProperty.Register(
            "Lessons",
            typeof(Lessons),
            typeof(SchedulePresenter),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnLessonsChanged)));

        private static void OnLessonsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SchedulePresenter schedulePresenter) return;
            schedulePresenter.SetUp();
        }

        public Lessons Lessons
        {
            get { return (Lessons)GetValue(LessonsProperty); }
            set { SetValue(LessonsProperty, value); }
        }

        #endregion

        #region CurrentWeekProperty

        public static readonly DependencyProperty CurrentWeekProperty = DependencyProperty.Register(
            "CurrentWeek",
            typeof(int),
            typeof(SchedulePresenter),
            new FrameworkPropertyMetadata(0, new PropertyChangedCallback(OnWeekChanged)));

        private static void OnWeekChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if(d is not SchedulePresenter schedulePresenter) return;
            if (e.NewValue != null)
            {
                schedulePresenter.ReturnButtonStatusUpdate();
                if(schedulePresenter._currentWeekLabel != null)
                {
                    schedulePresenter._currentWeekLabel.Text = "Неделя: " + schedulePresenter.CurrentWeek.ToString();
                }
                schedulePresenter.SetUp();
            }
        }

        public int CurrentWeek
        {
            get { return (int)GetValue(CurrentWeekProperty); }
            set { SetValue(CurrentWeekProperty, value); }
        }

        #endregion

        #region FirstSubGroupProperty

        public static readonly DependencyProperty FirstSubGroupProperty = DependencyProperty.Register(
            "FirstSubGroup",
            typeof(bool),
            typeof(SchedulePresenter),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnFirstSGChanged)));

        private static void OnFirstSGChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SchedulePresenter schedulePresenter) return;
            if(e.NewValue != null)
            {
                schedulePresenter.SetUp();
            }
        }

        public bool FirstSubGroup
        {
            get { return (bool)GetValue(FirstSubGroupProperty); }
            set { SetValue(FirstSubGroupProperty, value); }
        }

        #endregion

        #region SecondSubGroupProperty

        public static readonly DependencyProperty SecondSubGroupProperty = DependencyProperty.Register(
            "SecondSubGroup",
            typeof(bool),
            typeof(SchedulePresenter),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnSecondSGChanged)));

        private static void OnSecondSGChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SchedulePresenter schedulePresenter) return;
            if (e.NewValue != null)
            {
                schedulePresenter.SetUp();
            }
        }

        public bool SecondSubGroup
        {
            get { return (bool)GetValue(SecondSubGroupProperty); }
            set { SetValue(SecondSubGroupProperty, value); }
        }

        #endregion

        #region ShowExamsProperty

        public static readonly DependencyProperty ShowExamsProperty = DependencyProperty.Register(
            "ShowExams",
            typeof(bool),
            typeof(SchedulePresenter),
            new FrameworkPropertyMetadata(true, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, new PropertyChangedCallback(OnShowExamsChanged)));

        private static void OnShowExamsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SchedulePresenter schedulePresenter) return;
            if (e.NewValue != null)
            {
                schedulePresenter.SetUp();
            }
        }

        public bool ShowExams
        {
            get { return (bool)GetValue(ShowExamsProperty); }
            set { SetValue(ShowExamsProperty, value); }
        }

        #endregion

        #region CommandProperty

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(SchedulePresenter));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        #endregion

        #region WindowStateProperty

        public static readonly DependencyProperty WindowStateProperty = DependencyProperty.Register(
            "WindowState",
            typeof(WindowState),
            typeof(SchedulePresenter),
            new FrameworkPropertyMetadata(WindowState.Maximized, new PropertyChangedCallback(OnWindowStateChanged)));

        private static void OnWindowStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not SchedulePresenter schedulePresenter) return;
            if (schedulePresenter._maximizedView == null || schedulePresenter._scheduleView == null) return;
            if(e.NewValue != null)
            {
                schedulePresenter.SetUp();
            }
        }
        public WindowState WindowState
        {
            get { return (WindowState)GetValue(WindowStateProperty); }
            set { SetValue(WindowStateProperty, value); }
        }

        #endregion

        private void SetUp()
        {
            if(_scheduleView == null || _maximizedView == null || _normalView == null) return;

            CleanGrid();

            if (Lessons == null || Lessons.IsEmpty)
            {
                _scheduleView.SetValue(VisibilityProperty, Visibility.Collapsed);
                _normalView.SetValue(VisibilityProperty, Visibility.Collapsed);
                _emptyMessage.SetValue(VisibilityProperty, Visibility.Visible);
                return;
            }
            _emptyMessage.SetValue(VisibilityProperty, Visibility.Collapsed);

            if (WindowState == WindowState.Maximized)
                SetUpMaximized();
            else if (WindowState == WindowState.Normal)
                SetUpNormal();
        }

        #region SetUpMethods
        
        private void SetUpNormal()
        {
            SetUpDatesNormal();
            SetNormalSchedulePlates();
            _normalView.SetValue(VisibilityProperty, Visibility.Visible);
            _maximizedView.SetValue(VisibilityProperty, Visibility.Collapsed);
            _scheduleView.SetValue(VisibilityProperty, Visibility.Collapsed);
}
        private void SetUpMaximized()
        {
            GetWeeklyTimeRange();
            SetUpDatesMaximized();
            SetSchedulesTimes();
            SetMaximizedSchedulePlates();
            DeleteFreeRows();
            _normalView.SetValue(VisibilityProperty, Visibility.Collapsed);
            _maximizedView.SetValue(VisibilityProperty, Visibility.Visible);
            _scheduleView.SetValue(VisibilityProperty, Visibility.Visible);
}

        #endregion

        private void CleanGrid()
        {
            StackPanel? panel;
            for(int i= 0; i < _maximizedView.Children.Count; i++)
            {
                if (_maximizedView.Children[i] is not UIElement element) continue;
                if (element is Border) continue;
                if (element is TextBlock) continue;
                panel = element as StackPanel;
                if(panel != null) panel.Children.Clear();
                else
                {
                    _maximizedView.Children.Remove(element);
                    i--;
                }
            }
            for(int i = 0; i < _normalView.Children.Count; i++)
            {
                if (_normalView.Children[i] is not UIElement element) continue;
                if(element is Border) continue;
                panel = element as StackPanel;
                if(panel != null) panel.Children.Clear();
            }
        }

        #region SetupDatesMethods

        private void SetUpDatesMaximized()
        {
            if(_scheduleView == null) return;
            for(int i = 1; i < GRID_COLS; i++)
            {
                if (GetElementInGridPosition(_scheduleView ,i, 0, typeof(TextBlock)) is not TextBlock textBlock) continue;
                textBlock.Text = GetDateString(_dates[i - 1]);
            }
            if (_dates.Contains(DateTime.Today))
            {
                _todayBorder.SetValue(Grid.ColumnProperty, _dates.IndexOf(DateTime.Today) + 1);
                _todayBorder.SetValue(VisibilityProperty, Visibility.Visible);
            }
            else
            {
                _todayBorder.SetValue(VisibilityProperty, Visibility.Collapsed);
            }
        }
        private void SetUpDatesNormal()
        {
            if (_normalView == null) return;
            for(int i = 0; i < GRID_ROWS_NORMAL; i += 2)
            {
                if(GetElementInGridPosition(_normalView, 0, i, typeof(TextBlock)) is not TextBlock textBlock) continue;
                textBlock.Text = GetDateString(_dates[i / 2], true);
            }
        }

        #endregion

        private void GetWeeklyTimeRange()
        {
            TimeOnly minTime = TimeOnly.MaxValue;
            TimeOnly maxTime = TimeOnly.MinValue;
            TimeOnly sTime = TimeOnly.Parse("9:00");

            foreach(var prop in Lessons.GetType().GetProperties())
            {
                if (prop.GetValue(Lessons) is not List<Lesson> list || list.Count == 0) continue;
                var lList = list
                    .Where(i => (i.weekNumber != null && i.weekNumber.Contains(CurrentWeek)) || (DateTime.TryParse(i.startLessonDate, out DateTime date) && _dates.Contains(date)))
                    .Where(i => i.numSubgroup == 0 || (i.numSubgroup == 1 && FirstSubGroup) || (i.numSubgroup == 2 && SecondSubGroup)).ToList();
                foreach(var lesson in lList)
                {
                    if (lesson.announcement)
                        if(DateTime.TryParse(lesson.startLessonDate, out DateTime date))
                            if (!_dates.Contains(date))
                                continue;
                    if (!TimeOnly.TryParse(lesson.startLessonTime, out TimeOnly time)) continue;
                    if (time < sTime) minTime = sTime;
                    else if(time < minTime) minTime = time;
                    if(time > maxTime) maxTime = time;
                }
            }
            if (minTime == TimeOnly.MaxValue && maxTime == TimeOnly.MinValue)
            {
                _minRow = -1;
                _maxRow = -1;
                return;
            }
            _minRow = StartLessonDict[GetNearestTime(minTime)];
            _maxRow = StartLessonDict[GetNearestTime(maxTime)];
        }

        private TimeOnly GetNearestTime(TimeOnly time)
        {
            var bestMatch = StartLessonDict.OrderBy(e => Math.Abs((time - e.Key).TotalMinutes)).FirstOrDefault();
            if (bestMatch.Key == default)
                return TimeOnly.Parse("9:00");
            return bestMatch.Key;
        }
        private void SetSchedulesTimes()
        {
            if (_maximizedView == null) return;
            if(_minRow < 0 || _maxRow < 0) return;
            for(int i = _minRow; i <= _maxRow; i++)
            {
                ScheduleTime time = new ScheduleTime();
                time.SetValue(Grid.RowProperty, i);
                time.SetValue(Grid.ColumnProperty, 0);
                time.SetValue(ScheduleTime.StartTimeProperty, StartLessonDict.FirstOrDefault(x => x.Value == i).Key);
                _maximizedView.Children.Add(time);
            }
        }

        #region SetupGridMethods

        private void SetUpMaximizedGrid()
        {
            for(int i = 1; i < GRID_COLS; i++)
            {
                TextBlock textBlock = new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.DemiBold
                };
                textBlock.SetValue(Grid.ColumnProperty, i);
                textBlock.SetValue(Grid.RowProperty, 0);
                _scheduleView.Children.Add(textBlock);
            }
            for(int i = 0; i < GRID_ROWS; i++)
            {
                for(int j = 1; j < GRID_COLS; j++)
                {
                    StackPanel panel = new StackPanel();
                    panel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
                    panel.SetValue(Grid.RowProperty, i);
                    panel.SetValue(Grid.ColumnProperty, j);
                    _maximizedView.Children.Add(panel);
                }
            }
        }
        private void SetUpNormalGrid()
        {
            for(int i = 0; i < GRID_ROWS_NORMAL; i += 2)
            {
                TextBlock textBlock = new TextBlock()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    TextAlignment = TextAlignment.Center,
                    FontWeight = FontWeights.DemiBold
                };
                textBlock.SetValue(Grid.RowProperty, i);
                _normalView.Children.Add(textBlock);
            }
            for(int i = 1; i < GRID_ROWS_NORMAL; i += 2)
            {
                StackPanel panel = new StackPanel();
                panel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
                panel.SetValue(Grid.RowProperty, i);
                _normalView.Children.Add(panel);
            }
        }

        #endregion

        #region SetSchedulePlatesMethods

        private void SetMaximizedSchedulePlates()
        {
            int col = 0;
            int row = 0;
            foreach(var prop in Lessons.GetType().GetProperties())
            {
                col++;
                if (prop.GetValue(Lessons) is not List<Lesson> list || list.Count == 0) continue;
                var flist = list
                    .Where(i => (i.weekNumber != null && i.weekNumber.Contains(CurrentWeek)) || (DateTime.TryParse(i.startLessonDate, out DateTime date) && _dates.Contains(date)))
                    .Where(i => i.numSubgroup == 0 || (i.numSubgroup == 1 && FirstSubGroup) || (i.numSubgroup == 2 && SecondSubGroup));
                foreach(var item in flist)
                {
                    if (string.IsNullOrEmpty(item.startLessonTime)) continue;
                    if ((item.lessonTypeAbbrev == "Экзамен" || item.lessonTypeAbbrev == "Консультация") && _showExams.IsChecked == false)
                        continue;
                    if (item.announcement)
                        if(DateTime.TryParse(item.startLessonDate, out DateTime date))
                            if (!_dates.Contains(date))
                                continue;
                    if (!TimeOnly.TryParse(item.startLessonTime, out TimeOnly time)) continue;
                    row = StartLessonDict[GetNearestTime(time)];
                    if (GetElementInGridPosition(_maximizedView, col, row) is not StackPanel panel) continue;
                    SchedulePlate plate = new SchedulePlate();
                    plate.SetValue(Grid.RowProperty, row);
                    plate.SetValue(Grid.ColumnProperty, col);
                    plate.SetValue(SchedulePlate.ScheduleProperty, item);
                    plate.SetValue(SchedulePlate.CommandProperty, Command);
                    plate.SetValue(SchedulePlate.StateProperty, PlateState.Maximized);

                    panel.Children.Add(plate);
                }
            }
        }

        private void SetNormalSchedulePlates()
        {
            var properties = Lessons.GetType().GetProperties();
            for(int i = 0; i < properties.Length; i++)
            {
                if (GetElementInGridPosition(_normalView, 0, i * 2 + 1) is not StackPanel panel) continue;
                if (properties[i].GetValue(Lessons) is not List<Lesson> list || list.Count == 0)
                {
                    AddNoLessonTextBlock(panel);
                    continue;
                }
                var flist = list
                    .Where(i => (i.weekNumber != null && i.weekNumber.Contains(CurrentWeek)) || (DateTime.TryParse(i.startLessonDate, out DateTime date) && _dates.Contains(date)))
                    .Where(i => i.numSubgroup == 0 || (i.numSubgroup == 1 && FirstSubGroup) || (i.numSubgroup == 2 && SecondSubGroup))
                    .OrderBy(i => TimeOnly.TryParse(i.startLessonTime, out TimeOnly time) ? time : TimeOnly.MaxValue);
                if(flist.Count() == 0)
                {
                    AddNoLessonTextBlock(panel);
                    continue;
                }
                foreach (var item in flist)
                {
                    if (string.IsNullOrEmpty(item.startLessonTime)) continue;
                    if ((item.lessonTypeAbbrev == "Экзамен" || item.lessonTypeAbbrev == "Консультация") && _showExams.IsChecked == false)
                        continue;
                    if (item.announcement)
                        if (DateTime.TryParse(item.startLessonDate, out DateTime date))
                            if (!_dates.Contains(date))
                                continue;
                    SchedulePlate plate = new SchedulePlate();
                    plate.SetValue(SchedulePlate.ScheduleProperty, item);
                    plate.SetValue(SchedulePlate.CommandProperty, Command);
                    plate.SetValue(SchedulePlate.StateProperty, PlateState.Normal);

                    panel.Children.Add(plate);
                }
            }
        }

        private void AddNoLessonTextBlock(StackPanel panel)
        {
            TextBlock text = new TextBlock()
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(3),
                Height = 25,
                Text = "Нет занятий"
            };
            panel.Children.Add(text);
        }

        #endregion

        private void DeleteFreeRows()
        {
            bool flag = false;
            for (int i = _minRow; i <= _maxRow; i++)
            {
                for(int j = 1; j < GRID_COLS; j++)
                {
                    if (GetElementInGridPosition(_maximizedView, j, i, typeof(StackPanel)) is StackPanel panel && panel.Children.Count > 0)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    flag = false;
                    continue;
                }
                if (GetElementInGridPosition(_maximizedView, 0, i, typeof(ScheduleTime)) is ScheduleTime time) 
                    _maximizedView.Children.Remove(time);
            }
        }

        private UIElement? GetElementInGridPosition(Grid grid, int column, int row, Type? type = null)
        {
            if(type == null)
                return grid.Children
                              .Cast<UIElement>()
                              .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column);
            return grid.Children
                              .Cast<UIElement>()
                              .FirstOrDefault(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == column && e.GetType() == type);
        }

        private void ChangeWeek(int offset)
        {
            _weekDiff += offset;
            ChangeDates(offset);
            offset = offset % 4;
            if (offset == 0)
            {
                SetUp();
                ReturnButtonStatusUpdate();
                return;
            }
            CurrentWeek = (CurrentWeek + offset + 3) % 4 + 1;
        }
        private void BackCurrentWeek()
        {
            ChangeWeek(-_weekDiff);
        }

        private void ReturnButtonStatusUpdate()
        {
            if (_returnButton == null) return;
            _returnButton.Visibility = _weekDiff != 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private int GetWeekDiff(DateTime d1, DateTime d2, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            var diff = d2.Subtract(d1);

            var weeks = (int)diff.Days / 7;

            var remainingDays = diff.Days % 7;
            var cal = CultureInfo.InvariantCulture.Calendar;
            var d1WeekNo = cal.GetWeekOfYear(d1, CalendarWeekRule.FirstFullWeek, startOfWeek);
            var d1PlusRemainingWeekNo = cal.GetWeekOfYear(d1.AddDays(remainingDays), CalendarWeekRule.FirstFullWeek, startOfWeek);

            if (d1WeekNo != d1PlusRemainingWeekNo)
                weeks++;

            return weeks;
        }

        private List<DateTime> GetCurrentWeekDates()
        {
            DateTime today = DateTime.Today;
            CultureInfo culture = CultureInfo.CurrentCulture;
            int weekOffset = culture.DateTimeFormat.FirstDayOfWeek - today.DayOfWeek;
            if (today.DayOfWeek == 0)
                weekOffset -= 7;
            DateTime startOfWeek = today.AddDays(weekOffset);
            return Enumerable.Range(0, 6).Select(i => startOfWeek.AddDays(i)).ToList();
        }
        private void ChangeDates(int offset)
        {
            for(int i = 0; i < _dates.Count; i++)
            {
                _dates[i] = _dates[i].AddDays(offset * 7);
            }
        }

        private string GetDateString(DateTime date, bool linear = false)
        {
            DateTime dateTime = (DateTime)date;
            string day = dateTime.ToString("dddd");
            day = day.Replace(day[0], Char.ToUpper(day[0]));
            if(linear)
                day += " - " + dateTime.ToString("dd.MM");
            else
                day += "\n" + dateTime.ToString("dd.MM");
            if (dateTime == DateTime.Today)
            {
                day += " (сегодня)";
            }
            else if (dateTime == DateTime.Today.AddDays(1))
            {
                day += " (завтра)";
            }
            return day;
        }

        #region Events

        private void ShowExams_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox cb) return;
            ShowExams = (bool)cb.IsChecked!;
        }

        private void SecondSubGroup_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not CheckBox cb) return;
            SecondSubGroup = (bool)cb.IsChecked!;
        }

        private void FirstSubGroup_Click(object sender, RoutedEventArgs e)
        {
            if(sender is not CheckBox cb) return;
            FirstSubGroup = (bool)cb.IsChecked!;
        }

        private void ReturnButton_Click(object sender, RoutedEventArgs e)
        {
            BackCurrentWeek();
        }

        private void CalendarButton_Click(object sender, RoutedEventArgs e)
        {
            if(_calendarPopup == null) return;
            _calendarPopup.IsOpen = !_calendarPopup.IsOpen;
            _calendar.SelectedDate = _dates[0];
            _calendar.DisplayDate = _dates[0];
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeWeek(1);
        }

        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeWeek(-1);
        }

        private void Calendar_GotMouseCapture(object sender, MouseEventArgs e)
        {
            UIElement? originalElement = e.OriginalSource as UIElement;
            if (originalElement is CalendarDayButton || originalElement is CalendarItem)
            {
                originalElement.ReleaseMouseCapture();
            }
        }

        private void OpenDateButton_Click(object sender, RoutedEventArgs e)
        {
            if(_calendar == null || _calendarPopup == null) return;
            if (_calendar.SelectedDate is not DateTime date) return;
            int weeks = GetWeekDiff(date, _dates[0]);
            ChangeWeek(-weeks);
            _calendarPopup.IsOpen = false;
        }

        #endregion

        private const string PART_ScheduleView = "PART_ScheduleView";
        private const string PART_MaximizedView = "PART_MaximizedView";
        private const string PART_NormalScheduleView = "PART_NormalScheduleView";
        private const string PART_PreviousWeekButton = "PART_PreviousWeekButton";
        private const string PART_NextWeekButton = "PART_NextWeekButton";
        private const string PART_CalendarButton = "PART_CalendarButton";
        private const string PART_ReturnButton = "PART_ReturnButton";
        private const string PART_FirstSubGroup = "PART_FirstSubGroup";
        private const string PART_SecondSubGroup = "PART_SecondSubGroup";
        private const string PART_ShowExams = "PART_ShowExams";
        private const string PART_CurrentWeekLabel = "PART_CurrentWeekLabel";
        private const string PART_CalendarPopup = "PART_CalendarPopup";
        private const string PART_Calendar = "PART_Calendar";
        private const string PART_OpenDateButton = "PART_OpenDateButton";
        private const string PART_TodayBorder = "PART_TodayBorder";
        private const string PART_EmptyMessage = "PART_EmptyMessage";

        private Grid _scheduleView;
        private Grid _maximizedView;
        private Grid _normalView;
        private Button _previousButton;
        private Button _nextButton;
        private Button _calendarButton;
        private Button _returnButton;
        private CheckBox _firstSubGroup;
        private CheckBox _secondSubGroup;
        private CheckBox _showExams;
        private TextBlock _currentWeekLabel;
        private Popup _calendarPopup;
        private System.Windows.Controls.Calendar _calendar;
        private Button _openDateButton;
        private Border _todayBorder;
        private TextBlock _emptyMessage;
    }
}