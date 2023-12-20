using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BSUIRScheduleDESK.templates
{
    /// <summary>
    /// Interaction logic for EmployeeGroups.xaml
    /// </summary>
    public partial class EmployeeGroups : UserControl
    {
        public EmployeeGroups()
        {
            InitializeComponent();
        }

        #region Command

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command",
            typeof(ICommand),
            typeof(EmployeeGroups),
            new FrameworkPropertyMetadata(null));

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        #endregion
    }
}
