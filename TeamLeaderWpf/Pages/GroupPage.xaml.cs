using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
using TeamLeaderWpf.Mongo;

namespace TeamLeaderWpf.Pages
{
    /// <summary>
    /// Interaction logic for GroupPage.xaml
    /// </summary>
    public partial class GroupPage : Page
    {
        private bool _isChangeDuty1 = true;

        public int idSort {
            get { return _idSort; }
            set
            {
                if (value > 3)
                    value = 0;
                if(value == 0)
                    students = students.OrderBy(x => (x.LastName + x.FirstName + x.LastName)).ToList();
                else if(value == 1) 
                {
                    students = students.OrderBy(x => (x.LastName + x.FirstName + x.LastName)).ToList();
                    students = Enumerable.Reverse(students).ToList();
                }
                else if(value == 2) 
                    students = students.OrderBy(x => x.BirthDate).ToList();
                else if(value == 3)
                {
                    students = students.OrderBy(x => x.BirthDate).ToList();
                    students = Enumerable.Reverse(students).ToList();
                }

                _idSort = value;
                StudentsList.ItemsSource = students;
            }
        }

        private int _idSort;

        public List<ListStudent> students = new List<ListStudent>();
        public GroupPage()
        {
            InitializeComponent();
            NameGroupTextBlock.Text = MongoDb.currentGroup?.Name + " группа";
            
            var tempListSudnts = new List<Student>();

            foreach (var id in MongoDb.currentGroup?.StudentsId!)
            {
                bool isStudentIsDuty = false;

                if (id.ToString().Equals(Properties.Settings.Default.Duty1) || id.ToString().Equals(Properties.Settings.Default.Duty2))
                    isStudentIsDuty = true;

                students.Add(new ListStudent(MongoDb.FindStudentById(id), isStudentIsDuty));
            }

             

            StudentsList.ItemsSource = students;
            idSort = 0;


        }

        private void EditingDataButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new EditingDataPage());
        }

        private void OpenReportsPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ReportsPage());
        }

        private void OpenSummariesButtonPageClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = MongoDb.currentUser!.GoogleSummaryPage,
                UseShellExecute = true
            });
        }

        private void FormSixButtonClick(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = MongoDb.currentUser!.GoogleFormSix,
                UseShellExecute = true
            });
        }

        private void SortFullNameButtonClick(object sender, RoutedEventArgs e)
        {
            if(idSort == 0)
            {
                idSort++;
                SortBirthDateButton.Content = "Дата рождения";
                SortFullNameButton.Content = "ФИО 🠕";
            }
            else
            {
                idSort = 0;
                SortBirthDateButton.Content = "Дата рождения";
                SortFullNameButton.Content = "ФИО 🠗";
            }
        }

        private void SortBirthDateButton_Click(object sender, RoutedEventArgs e)
        {
            if (idSort == 2)
            {
                idSort++;
                SortBirthDateButton.Content = "Дата рождения🠗";
                SortFullNameButton.Content = "ФИО";
            }
            else
            {
                idSort = 2;
                SortBirthDateButton.Content = "Дата рождения🠕";
                SortFullNameButton.Content = "ФИО";
            }
        }

        private void Page_Initialized(object sender, EventArgs e)
        {
            var dateTimeNow = DateTime.Now;
            var dayInMonth = DateTime.DaysInMonth(dateTimeNow.Year, dateTimeNow.Month);
            var daysNumberUntilMonthEnd = dayInMonth - dateTimeNow.Day;

            if (daysNumberUntilMonthEnd <= 5 && !Properties.Settings.Default.isReminderAlreadyShown)
            {
                MessageBox.Show($"До окончания сдачи сводника осталось {daysNumberUntilMonthEnd} дней", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                Properties.Settings.Default.isReminderAlreadyShown = true;
                Properties.Settings.Default.Save();
            }
        }

        private void GroupList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var student = StudentsList.SelectedItem as ListStudent;
            var id = student!.Id;

            if(!id.ToString().Equals(Properties.Settings.Default.Duty1) ||
                !id.ToString().Equals(Properties.Settings.Default.Duty2))
            {
                var idDuty = _isChangeDuty1 ? Properties.Settings.Default.Duty1 : Properties.Settings.Default.Duty2;
                
                foreach (var st in students)
                {
                    if (st.Id.ToString().Equals(idDuty))
                    {
                        st.IsDuty = false;
                        break;
                    }
                }
                if(_isChangeDuty1)
                    Properties.Settings.Default.Duty1 = id.ToString();
                else
                    Properties.Settings.Default.Duty2 = id.ToString();

                _isChangeDuty1 = !_isChangeDuty1;

                student.IsDuty = true;
                
                Properties.Settings.Default.Save();

                StudentsList.ItemsSource = students;
                StudentsList.Items.Refresh();
            }
        }
    }
}
