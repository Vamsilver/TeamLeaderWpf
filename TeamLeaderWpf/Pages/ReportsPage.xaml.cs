using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamLeaderWpf.Core;

namespace TeamLeaderWpf.Pages
{
    /// <summary>
    /// Interaction logic for ReportsPage.xaml
    /// </summary>
    public partial class ReportsPage : Page
    {
        string[] Months = { "Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь", "Любой"};
        List<Report> AllReports;
        List<Report> DisplayedReports;
        public bool IsOldFirst
        {
            get { return _isOldFirst; }
            set 
            {
                DisplayedReports = Enumerable.Reverse(DisplayedReports).ToList();
                ReportsList.ItemsSource = DisplayedReports;
                _isOldFirst = value;
            }
        }
        private bool _isOldFirst;
        public ReportsPage()
        {
            InitializeComponent();
            AllReports = new List<Report>(Mongo.MongoDb.GetAllReportsInCurrentGroup());

            AllReports.Sort((x, y) => y.CurrentDate.CompareTo(x.CurrentDate));

            DisplayedReports = AllReports;
            ReportsList.ItemsSource = DisplayedReports;
            MonthComboBox.ItemsSource = Months;
            MonthComboBox.SelectedIndex = 12;
        }

        private void AddewReportCLick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportPage());
        }

        private void ReportsList_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = ReportsList.SelectedItem;

            if (selectedItem != null)
            {
                NavigationService.Navigate(new ReportPage(selectedItem as Report));
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GroupPage());
        }

        private void NewSortListButton_Click(object sender, RoutedEventArgs e)
        {
            if(IsOldFirst)
            {
                NewSortListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#005095");
                NewSortListButton.Foreground = new SolidColorBrush(Colors.White);
                OldSortListButton.Background = new SolidColorBrush(Colors.White);
                OldSortListButton.Foreground = new SolidColorBrush(Colors.Black);
                IsOldFirst = !IsOldFirst;
            }
        }

        private void MonthComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillListBox();
        }

        private void FillListBox()
        {
            var curMonth = MonthComboBox.SelectedIndex;
            if (curMonth != -1)
            {
                if (curMonth != 12)
                    DisplayedReports = AllReports.Where(x => x.CurrentDate.Month == curMonth + 1).ToList();
                else
                    DisplayedReports = AllReports;
            
                if(IsOldFirst)
                {
                    DisplayedReports.Sort((x, y) => y.CurrentDate.CompareTo(x.CurrentDate));
                    DisplayedReports.Reverse();
                }
                else
                {
                    DisplayedReports.Sort((x, y) => y.CurrentDate.CompareTo(x.CurrentDate));
                }
            }

            ReportsList.ItemsSource = DisplayedReports;
        }

        private void OldSortListButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsOldFirst)
            {
                NewSortListButton.Background = new SolidColorBrush(Colors.White);
                NewSortListButton.Foreground = new SolidColorBrush(Colors.Black);
                OldSortListButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#005095");
                OldSortListButton.Foreground = new SolidColorBrush(Colors.White);
                IsOldFirst = !IsOldFirst;
            }
        }

        private void ReportsList_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            var report = ReportsList.SelectedItem as Report;

            if(report != null)
            {
                var msg = MessageBox.Show($"Вы уверены, что хотите удалить рапортичку за " +
                $"{(report.CurrentDate.Day < 10 ? "0" + report.CurrentDate.Day: report.CurrentDate.Day)}." +
                $"{(report.CurrentDate.Month < 10 ? "0" + report.CurrentDate.Month : report.CurrentDate.Month)}." +
                $"{report.CurrentDate.Year}?", "Подвтердите действие", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                if(msg == MessageBoxResult.OK) 
                {
                    Mongo.MongoDb.DeleteOneReport(report);
                    MessageBox.Show("Удаление прошло успешно", "Успех");
                    NavigationService.Navigate(new ReportsPage());
                }

            }
        }
    }
}
