using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Linq;
using TeamLeaderWpf.Core;

namespace TeamLeaderWpf.Pages
{
    /// <summary>
    /// Interaction logic for RecordPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        bool IsCreateNewReport;
        string[] Months = {"Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь", };
        DateOnly CurrentDateTime;
        List<int> Days = new List<int>();

        public List<Student> AllStudents = new List<Student>();
        Report currentReport;
        ObservableCollection<TabRecord> tabRecords = new ObservableCollection<TabRecord>();

        public ReportPage()
        {
            InitializeComponent();
            currentReport = new Report();
            RecordsList.ItemsSource = tabRecords;
            MonthsComboBox.ItemsSource = Months;
            CurrentDateTime = DateOnly.FromDateTime(DateTime.Now);
            IsCreateNewReport = true;

            for (int i = 1; i <= DateTime.DaysInMonth(CurrentDateTime.Year, CurrentDateTime.Month); i++)
            {
                Days.Add(i);
            }

            DaysComboBox.ItemsSource = Days;

            MonthsComboBox.SelectedIndex = CurrentDateTime.Month - 1;
            DaysComboBox.SelectedIndex = CurrentDateTime.Day - 1;

            YearTextBlock.Text = " " + CurrentDateTime.Year;

            AllStudents = Mongo.MongoDb.GetAllStudentsInCurrentGroup();
        }

        public ReportPage(Report report)
        {
            InitializeComponent();
            currentReport = report;
            AllStudents = Mongo.MongoDb.GetAllStudentsInCurrentGroup();
            IsCreateNewReport = false;
            CurrentDateTime = report.CurrentDate;

            foreach (var rep in report.Records)
            {
                var tabRep = new TabRecord(AllStudents, rep.StudentId);
                tabRep.FirstLesson = rep.FirstLesson;
                tabRep.SecondLesson = rep.SecondLesson;
                tabRep.ThirdLesson = rep.ThirdLesson;
                tabRep.FourthLesson = rep.FourthLesson;
                tabRep.TotalHours = rep.TotalHours;
                tabRecords.Add(tabRep);
            }
            RecordsList.ItemsSource = tabRecords;
            MonthsComboBox.ItemsSource = Months;

            MonthsComboBox.SelectedIndex = report.CurrentDate.Month - 1;

            for (int i = 1; i <= DateTime.DaysInMonth(CurrentDateTime.Year, CurrentDateTime.Month); i++)
            {
                Days.Add(i);
            }
            DaysComboBox.SelectedIndex = report.CurrentDate.Day - 1;

            YearTextBlock.Text = " " + CurrentDateTime.Year;

            DaysComboBox.ItemsSource = Days;
        }

        private void AddNewRecordClick(object sender, RoutedEventArgs e)
        {
            tabRecords.Add(new TabRecord(AllStudents));
        }

        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            bool isReportNotSave = false;

            currentReport.GroupId = Mongo.MongoDb.currentGroup!.Id;
            currentReport.CurrentDate = CurrentDateTime;

            if (IsCreateNewReport)
            {
                if (CheckReportDate(IsCreateNewReport) && FillCurrentReport())
                {
                    Mongo.MongoDb.AddReportToDataBase(currentReport);
                }
                else
                {
                    isReportNotSave = true;
                }
            }
            else
            {
                if (!CheckReportDate(IsCreateNewReport))
                {
                    var msg = MessageBox.Show($"Вы уверены, что хотите " +
                        $"перезаписать рапортичку за " +
                        $"{(CurrentDateTime.Day < 10 ? "0" + CurrentDateTime.Day : CurrentDateTime.Day)}." +
                        $"{(CurrentDateTime.Month < 10 ? "0" + CurrentDateTime.Month : CurrentDateTime.Month)}." +
                        $"{CurrentDateTime.Year}?",
                        "Подтвердите действие", MessageBoxButton.OKCancel, MessageBoxImage.Question);

                    if (msg == MessageBoxResult.Cancel)
                        return;
                }
                if (FillCurrentReport())
                    Mongo.MongoDb.ReplaceOneReport(currentReport);
                else
                {
                    isReportNotSave = true;
                }
            }

            if(!isReportNotSave)
            {
                MessageBox.Show("Рапортичка успешно заполнена!", "Успех", MessageBoxButton.OK);
                NavigationService.Navigate(new ReportsPage());
            }
        }

        private bool CheckReportDate(bool isCreateNewReport)
        {
            var allReports = Mongo.MongoDb.GetAllReportsInCurrentGroup();
            if (allReports.Where(z => z.CurrentDate == currentReport.CurrentDate).FirstOrDefault() != null)
            {
                if(isCreateNewReport) 
                    MessageBox.Show("Эта дата уже заполнена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private bool FillCurrentReport()
        {
            bool IsOkayToRemoveIncorrectStudens = false;

            List<Record> records = new List<Record>();
            foreach (var s in tabRecords)
            {
                if (s.Student != null)
                {
                    var rec = new Record(s.Student);
                    rec.FirstLesson = s.FirstLesson;
                    rec.SecondLesson = s.SecondLesson;
                    rec.ThirdLesson = s.ThirdLesson;
                    rec.FourthLesson = s.FourthLesson;
                    rec.TotalHours = s.TotalHours;
                    records.Add(rec);
                }
                else if (!IsOkayToRemoveIncorrectStudens)
                {
                    var res = MessageBox.Show("Один или несколько студентов указаны не корректно, если они вам не нужны " +
                        "(рапортичка заполнить без них), то нажмите \"OK\", иначе \"Отмена\", чтобы заполнить рапортичку до конца",
                        "Ошибка", MessageBoxButton.OKCancel, MessageBoxImage.Error);

                    if (res == MessageBoxResult.Cancel)
                    {
                        return false;
                    }
                    else
                    {
                        IsOkayToRemoveIncorrectStudens = true;
                    }
                }
            }

            currentReport.Records = records;
            return true;
        }

        private void ChangeLessonChecbox(object sender, RoutedEventArgs e)
        {
            RecordsList.Items.Refresh();
        }

        private void DeleteButtonClick(object sender, RoutedEventArgs e)
        {
            var container = FindParentOfType<ListBoxItem>(sender as Button);
            RecordsList.SelectedItem = container;
            tabRecords.Remove(RecordsList.SelectedItem as TabRecord);
        }
        private static T FindParentOfType<T>(FrameworkElement element) where T : FrameworkElement
        {
            while (element != null)
            {
                if (element is T t)
                    return t;
                element = (FrameworkElement)VisualTreeHelper.GetParent(element);
            }
            return null;
        }

        private static void GetLogicalChildCollection<T>(DependencyObject parent, List<T> logicalCollection) where T : DependencyObject
        {
            IEnumerable children = LogicalTreeHelper.GetChildren(parent);
            foreach (object child in children)
            {
                if (child is DependencyObject)
                {
                    DependencyObject depChild = child as DependencyObject;
                    if (child is T)
                    {
                        logicalCollection.Add(child as T);
                    }
                    GetLogicalChildCollection(depChild, logicalCollection);
                }
            }
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            MessageBoxResult res;

            if (!IsCreateNewReport)
                res = MessageBox.Show("Вы уверены, что хотите выйти? Изменения не сохранятся.", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Hand);
            else
                res = MessageBox.Show("Вы уверены, что хотите выйти? Данные не сохранятся.", "Предупреждение", MessageBoxButton.OKCancel, MessageBoxImage.Hand);
            if(res == MessageBoxResult.OK)
                NavigationService.Navigate(new ReportsPage());
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = currentReport;
        }

        private void StudentLastNameComboBox_Loaded(object sender, RoutedEventArgs e)
        {
            var container = FindParentOfType<ListBoxItem>(sender as ComboBox);
            var selectedStudentInComboBox = (container.Content as TabRecord).Student;
            if (selectedStudentInComboBox != null)
            {
                var sd = AllStudents.FindIndex(z => z.Id == selectedStudentInComboBox.Id);
                (sender as ComboBox).SelectedIndex = sd;
            }
        }

        private void DaysComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int day = (sender as ComboBox).SelectedIndex + 1;
            int month = MonthsComboBox.SelectedIndex + 1;
            CurrentDateTime = new DateOnly(DateTime.Now.Year, month, day);
        }

        private void MonthsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var curMonthIndex = MonthsComboBox.SelectedIndex;
            var curDayIndex = DaysComboBox.SelectedIndex;

            if (curMonthIndex != -1 && curDayIndex != -1)
            {
                Days.Clear();
                for (int i = 1; i <= DateTime.DaysInMonth(CurrentDateTime.Year, curMonthIndex + 1); i++)
                {
                    Days.Add(i);
                }

                if(DaysComboBox.SelectedIndex > Days.Count - 1)
                {
                    DaysComboBox.SelectedIndex = Days.Count - 1;
                }

                DaysComboBox.ItemsSource = Days;
                DaysComboBox.Items.Refresh();

                int day = DaysComboBox.SelectedIndex + 1;
                int month = curMonthIndex + 1;
                CurrentDateTime = new DateOnly(DateTime.Now.Year, month, day);
            }
        }

        private void StudentLastNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selItem = (sender as ComboBox).SelectedItem;

            if (selItem != null)
            {
                List<Student> currStudents = new List<Student>();
                
                foreach (var rec in tabRecords)
                {
                    if (rec.Student != null)
                        currStudents.Add(rec.Student);
                }
                
                if (currStudents.Where(z => z.ToString().Equals(selItem.ToString())).ToList().Count > 1)
                {
                    MessageBox.Show("Невозомжно записать два раза одного и того же студента!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    (sender as ComboBox).SelectedIndex = -1;
                }
            }
        }
    }

    public class TabRecord : Record
    {
        public TabRecord(List<Student> students) 
        { 
            Students = students;
            CheckStudentId();
        }

        public TabRecord(List<Student> students, ObjectId studentId)
        {
            Students = students;
            StudentId = studentId;
            Student = Mongo.MongoDb.FindStudentById(studentId);
            CheckStudentId();
        }

        private void CheckStudentId()
        {
            if (StudentId != null)
            {
                Student = Mongo.MongoDb.FindStudentById(StudentId);
            }
        }

        public List<Student> Students { get; set; }
        public Student Student { get; set; }
    }

}
