using System;
using System.Windows;
using System.Windows.Media;

using Arash;


namespace PersianDateDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.datePicker.DisplayDateStart = this.calendar.DisplayDateStart =
                (new PersianDate(1, 1, 1)).ToDateTime();
            this.persianDatePicker.DisplayDateEnd = this.persianCalendar.DisplayDateEnd = new PersianDate(DateTime.MaxValue);
            this.datePicker.DisplayDateEnd = this.calendar.DisplayDateEnd = DateTime.MaxValue;

            var originalBackground = this.Background;
            try
            {
                this.Background = new SolidColorBrush(new Color() { A = 128, R = 255, G = 255, B = 255 });
                Arash.AeroGlassEffectUtility.ExtendGlass(this, -1, -1, -1, -1);
            }
            catch (Exception)
            {
                this.Background = originalBackground;
            }
        }
    }
}
