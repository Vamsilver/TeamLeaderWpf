using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
using TeamLeaderWpf.Pages.DataPages;

namespace TeamLeaderWpf.Pages
{
    public partial class EditingDataPage : Page
    {
        ContactsDataPage contactsDataPage = new ContactsDataPage();
        public EditingDataPage()
        {
            InitializeComponent();
            EditFrame.Navigate(contactsDataPage);
        }

        private void BackButtonClick(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GroupPage());
        }

        private void SaveChangesButtonClick(object sender, RoutedEventArgs e)
        {
            try
            {
                List<TabStudent> students = contactsDataPage.GetTabsStudents();
                foreach (TabStudent student in students)
                {
                    Mongo.MongoDb.ReplaceOneStudentInDataBase(student);
                }
            }
            catch
            {
                return;
            }

            MessageBox.Show("Данные успешно сохранены!");
        }

        private void CreateFileClick(object sender, RoutedEventArgs e)
        {
            contactsDataPage.CreateFile();
        }
    }
}
