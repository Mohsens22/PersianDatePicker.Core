using Mohsen.PersianDateControls;
using System;
using System.Windows;

namespace Mohsen.PersianDateControls.SampleProject;
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
    }
}
