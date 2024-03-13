using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BSUIRScheduleDESK.templates
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        #region MaxDropDownHeigth

        public static readonly DependencyProperty MaxDropDownHeigthProperty = DependencyProperty.Register(
            "MaxDropDownHeigth",
            typeof(double),
            typeof(SearchBox),
            new FrameworkPropertyMetadata(
                0D,
                FrameworkPropertyMetadataOptions.AffectsArrange |
                FrameworkPropertyMetadataOptions.AffectsRender));


        public double MaxDropDownHeigth
        {
            get { return (double)GetValue(MaxDropDownHeigthProperty); }
            set { SetValue(MaxDropDownHeigthProperty, value); }
        }
        #endregion

        #region ItemsSource

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
            "ItemsSource",
            typeof(IEnumerable),
            typeof(SearchBox),
            new FrameworkPropertyMetadata(
                new PropertyChangedCallback(OnItemsSourcePropertyChanged)));

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var control = sender as SearchBox;
            if (control != null)
            {
                if (e.OldValue is INotifyCollectionChanged oldValueINotifyCollectionChanged)
                {
                    oldValueINotifyCollectionChanged.CollectionChanged -= control.ItemsSource_CollectionChanged;
                }
                if (e.NewValue is INotifyCollectionChanged newValueINotifyCollectionChanged)
                {
                    newValueINotifyCollectionChanged.CollectionChanged += control.ItemsSource_CollectionChanged;
                }
            }
        }

        private void ItemsSource_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {

        }

        #endregion

        #region SText

        public static readonly DependencyProperty STextProperty = DependencyProperty.Register(
            nameof(SText),
            typeof(string),
            typeof(SearchBox),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public string SText
        {
            get { return (string)GetValue(STextProperty); }
            set { SetValue(STextProperty, value); }
        }

        #endregion

        #region IsDropDownOpen

        public static readonly DependencyProperty IsDropDownOpenProperty = DependencyProperty.Register(
            "IsDropDownOpen",
            typeof(bool),
            typeof(SearchBox),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsRender));

        public bool IsDropDownOpen
        {
            get { return (bool)GetValue(IsDropDownOpenProperty); }
            set { SetValue(IsDropDownOpenProperty, value); }
        }
        #endregion

        #region SelectedIndex

        public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(
            "SelectedIndex",
            typeof(int),
            typeof(SearchBox),
            new FrameworkPropertyMetadata(-1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }
        #endregion

        #region SelectedItem

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register(
            "SelectedItem",
            typeof(object),
            typeof(SearchBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }
        #endregion

        public SearchBox()
        {
            InitializeComponent();
        }

        bool ListFocused = false;
        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Up || e.Key == Key.Down)
            {
                if (!ListFocused)
                {
                    ListView.Focus();
                    ListFocused = true;
                }
            }
            else if (e.Key == Key.Enter)
            {
                return;
            }
            else
            {
                if (ListFocused)
                {
                    searchTextBox.Focus();
                    ListFocused= false;
                }
            }
        }

        private void ListView_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                ItemSelected.Invoke();
            }
        }

        public event Action ItemSelected;
    }
}
