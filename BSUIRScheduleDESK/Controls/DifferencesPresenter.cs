using BSUIRScheduleDESK.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BSUIRScheduleDESK.Controls
{
    public class DifferencesPresenter : Control
    {
        private const int COL_NUM = 2;
        private const double LEFT_MARGIN = 15;
        private const int FONT_SIZE = 13;

        static DifferencesPresenter()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DifferencesPresenter), new FrameworkPropertyMetadata(typeof(DifferencesPresenter)));
        }

        #region Differences

        public static readonly DependencyProperty DifferencesProperty = DependencyProperty.Register(
            "Differences",
            typeof(Difference),
            typeof(DifferencesPresenter),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDifferencesChanged)));

        private static void OnDifferencesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not DifferencesPresenter p) return;
            if(e.NewValue != null)
            {
                p.SetUpDifferences(p.Differences);
            }
        }

        public Difference Differences
        {
            get { return (Difference)GetValue(DifferencesProperty); }
            set { SetValue(DifferencesProperty, value); }
        }

        #endregion

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _logFieldPanel = (StackPanel)GetTemplateChild(PART_LogField);
            OldValueBackgroundBrush = (SolidColorBrush)TryFindResource(RESOURCE_OldValueBackgroundBrush);
            NewValueBackgroundBrush = (SolidColorBrush)TryFindResource(RESOURCE_NewValueBackgroundBrush);
            ForegroundBrush = (SolidColorBrush)TryFindResource(RESOURCE_TextForegroundBrush);

            if (Differences != null)
                SetUpDifferences(Differences);
        }
        private string[] CreateStrings(Difference difference)
        {
            string[] result = new string[COL_NUM] {string.Empty, string.Empty };
            if(difference == null) return result;
            if(difference.OldValue == null && difference.NewValue == null)
            {
                result[0] += difference.PropertyName;
                result[1] += difference.PropertyName;
            }
            else
            {
                result[0] += $"{(string.IsNullOrEmpty(difference.PropertyName) ? "" : difference.PropertyName + ": ")}{(difference.OldValue == null ? "null" : difference.OldValue.ToString())}";
                result[1] += $"{(string.IsNullOrEmpty(difference.PropertyName) ? "" : difference.PropertyName + ": ")}{(difference.NewValue == null ? "null" : difference.NewValue.ToString())}";
            }
            return result;
        }
        private TextBlock CreateTextBlockOld(string? text, int marginC, int type)
        {
            TextBlock result = new TextBlock();
            if (string.IsNullOrEmpty(text)) return result;

            result.Text = text;
            result.HorizontalAlignment = HorizontalAlignment.Stretch;
            result.Margin = new Thickness(marginC * LEFT_MARGIN, 1,0,1);
            result.Foreground = type == 0 ? ForegroundBrush : Brushes.White;
            result.FontSize = FONT_SIZE;
            result.TextWrapping = TextWrapping.Wrap;
            result.Background = type == 0 ? Brushes.Transparent : OldValueBackgroundBrush;

            return result;
        }
        private TextBlock CreateTextBlockNew(string text, int marginC, int type)
        {
            TextBlock result = new TextBlock();
            if (string.IsNullOrEmpty(text)) return result;

            result.Text = text;
            result.HorizontalAlignment = HorizontalAlignment.Stretch;
            result.Margin = new Thickness(marginC * LEFT_MARGIN, 1, 0, 1);
            result.Foreground = type == 0 ? ForegroundBrush : Brushes.White;
            result.FontSize = FONT_SIZE;
            result.TextWrapping = TextWrapping.Wrap;
            result.Background = type == 0 ? Brushes.Transparent : NewValueBackgroundBrush;

            return result;
        }
        private void SetUpDifferences(Difference difference, int marginC = 1)
        {
            if (_logFieldPanel == null) return;
            marginC++;
            foreach (Difference diff in difference.Differences)
            {
                string[] rowTexts = CreateStrings(diff);
                int lineType = GetLineType(diff.NewValue, diff.OldValue);
                if (string.IsNullOrEmpty(diff.PropertyName) && diff.OldValue == null)
                {
                    _logFieldPanel.Children.Add(CreateTextBlockNew(rowTexts[1], marginC, lineType));
                }
                else if (string.IsNullOrEmpty(diff.PropertyName) && diff.NewValue == null)
                {
                    _logFieldPanel.Children.Add(CreateTextBlockOld(rowTexts[0], marginC, lineType));
                }
                else if (rowTexts[0] == rowTexts[1])
                {
                    _logFieldPanel.Children.Add(CreateTextBlockOld(rowTexts[0], marginC, lineType));
                }
                else
                {
                    _logFieldPanel.Children.Add(CreateTextBlockOld(rowTexts[0], marginC, lineType));
                    _logFieldPanel.Children.Add(CreateTextBlockNew(rowTexts[1], marginC, lineType));
                }
                if (diff.Differences.Count == 0) continue;
                SetUpDifferences(diff, marginC);
            }
        }


        private int GetLineType(object? left, object? right)
        {
            if (left == null && right == null) return 0;
            if (left == null || right == null) return 1;
            return left.ToString() == right.ToString() ? 0 : 1;
        }

        private const string PART_LogField = "PART_LogField";
        private const string RESOURCE_OldValueBackgroundBrush = "OldValue.Background.Static";
        private const string RESOURCE_NewValueBackgroundBrush = "NewValue.Background.Static";
        private const string RESOURCE_TextForegroundBrush = "TextBlock.Foreground.Static";

        private StackPanel _logFieldPanel;
        private SolidColorBrush OldValueBackgroundBrush;
        private SolidColorBrush NewValueBackgroundBrush;
        private SolidColorBrush ForegroundBrush;
    }
}