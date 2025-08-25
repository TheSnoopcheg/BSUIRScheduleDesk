using System;
using System.Windows;
using System.Windows.Controls;

namespace BSUIRScheduleDESK.Controls;

public class ScheduleTime : Control
{
    static ScheduleTime()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(ScheduleTime), new FrameworkPropertyMetadata(typeof(ScheduleTime)));
    }

    public ScheduleTime() : base() { }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _startTime = (TextBlock)GetTemplateChild(PART_StartTimeLabel);
        _endTime = (TextBlock)GetTemplateChild(PART_EndTimeLabel);
        SetUpLabels();
    }

    private void SetUpLabels()
    {
        _startTime.Text = StartTime.ToString();
        _startTime.FontSize = StartTimeFontSize;
        TimeOnly endTime = StartTime.AddHours(1).AddMinutes(25);
        _endTime.Text = endTime.ToString();
        _endTime.FontSize = StartTimeFontSize - 4;
    }

    #region StartTimeProperty

    public static readonly DependencyProperty StartTimeProperty = DependencyProperty.Register(
        "StartTime",
        typeof(TimeOnly),
        typeof(ScheduleTime));
    public TimeOnly StartTime
    {
        get { return (TimeOnly)GetValue(StartTimeProperty); }
        set {  SetValue(StartTimeProperty, value); }
    }

    #endregion

    #region StartTimeFontSizeProperty

    public static readonly DependencyProperty StartTimeFontSizeProperty = DependencyProperty.Register(
        "StartTimeFontSize",
        typeof(double),
        typeof(ScheduleTime),
        new FrameworkPropertyMetadata(16d));

    public double StartTimeFontSize
    {
        get { return (double)GetValue(StartTimeFontSizeProperty); }
        set { SetValue(StartTimeFontSizeProperty, value); }
    }

    #endregion

    private const string PART_StartTimeLabel = "PART_StartTimeLabel";
    private const string PART_EndTimeLabel = "PART_EndTimeLabel";

    private TextBlock _startTime;
    private TextBlock _endTime;
}
