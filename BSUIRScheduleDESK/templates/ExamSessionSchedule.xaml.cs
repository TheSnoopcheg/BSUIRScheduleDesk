using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BSUIRScheduleDESK.templates
{
    /// <summary>
    /// Interaction logic for ExamSessionSchedule.xaml
    /// </summary>
    public partial class ExamSessionSchedule : UserControl
    {
        public ExamSessionSchedule()
        {
            InitializeComponent();
        }

        #region LoadScheduleCommand

        public static readonly DependencyProperty LoadScheduleCommandProperty = DependencyProperty.Register(
            "LoadScheduleCommand",
            typeof(ICommand),
            typeof(ExamSessionSchedule),
            new FrameworkPropertyMetadata(null));

        public ICommand LoadScheduleCommand
        {
            get { return (ICommand)GetValue(LoadScheduleCommandProperty); }
            set { SetValue(LoadScheduleCommandProperty, value); }
        }

        #endregion
    }
}
