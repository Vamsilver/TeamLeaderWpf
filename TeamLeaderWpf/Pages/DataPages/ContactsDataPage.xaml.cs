using Aspose.Cells;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using TeamLeaderWpf.Core;

namespace TeamLeaderWpf.Pages.DataPages
{
    /// <summary>
    /// Interaction logic for ContactsDataPage.xaml
    /// </summary>
    public partial class ContactsDataPage : System.Windows.Controls.Page
    {
        List<Student> students;
        public static List<TabStudent> tabsStudents;

        public ContactsDataPage()
        {
            InitializeComponent();

            students = Mongo.MongoDb.FindStudentsByIdList(Mongo.MongoDb.currentGroup.StudentsId).OrderBy(x => x.LastName + x.FirstName + x.MiddleName).ToList();
            tabsStudents = new List<TabStudent>();

        }

        private void studentsDataGrid_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            int count = 1;
            foreach (var st in students)
            {
                tabsStudents.Add(new TabStudent(st.Id, count, st.FirstName, st.LastName, st.MiddleName,
                    st.Gender, st.BirthDate, st.BirthPlace, st.PostCode, st.DistrictByRegistartion,
                    st.RegisteredAddress, st.ResidenceAddress, st.Citizenship, st.PassportData, st.ITN, st.INLILA,
                    st.MedicalPolicy, st.PhoneNumber, st.FatherFullName, st.FatherPhoneNumber, st.MotherFullName, 
                    st.MotherPhoneNumber));
                count++;
            }

            studentsDataGrid.ItemsSource = tabsStudents;
        }

        public List<TabStudent> GetTabsStudents()
        {
            return tabsStudents;
        }

        public void CreateFile()
        {
            var d = studentsDataGrid.ItemsSource.Cast<TabStudent>();
            var data = ToDataTable(d.ToList());
            var dlg = new SaveFileDialog();
            if(dlg.ShowDialog() == true)
            {
                ToExcelFile(data, dlg.FileName);
            }
        }

        public static System.Data.DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new System.Data.DataTable(typeof(T).Name);

            //Get all the properties
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in properties)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (var item in items)
            {
                var values = new object[properties.Length];
                for (var i = 0; i < properties.Length; i++)
                {
                    //inserting property values to data table rows
                    values[i] = properties[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check data table
            return dataTable;
        }

        public static void ToExcelFile(System.Data.DataTable dataTable, string filePath, bool overwriteFile = true)
        {
            if (File.Exists(filePath) && overwriteFile)
                File.Delete(filePath);

            using (var connection = new OleDbConnection())
            {
                connection.ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={filePath};" +
                                              "Extended Properties='Excel 12.0 Xml;HDR=YES;'";
                connection.Open();
                using (var command = new OleDbCommand())
                {
                    command.Connection = connection;
                    var columnNames = new List<string>() { "№", "Фамилия", "Имя", "Отчество", "Пол", "Дата рождения", "Место рождения", "Почтовый индекс", "Административный(муниципальный) район по прописке", "Адрес по прописке", "Адрес проживания", "Гражданство", "Паспортные данные (серия, номер, кем и когда выдан)", "ИНН", "Страховое свидетельство", "Медицинский полис", "Телефон обучающегося", "ФИО папы", "Телефон папы", "ФИО мамы", "Телефон мамы" };
                    //var columnNames = (from DataColumn dataColumn in dataTable.Columns select dataColumn.ColumnName).ToList();
                    var tableName = !string.IsNullOrWhiteSpace(dataTable.TableName) ? dataTable.TableName : Guid.NewGuid().ToString();
                    //var itemToRemove = columnNames.Single(x => x.Equals("Id"));
                    //columnNames.Remove(itemToRemove);
                    command.CommandText = $"CREATE TABLE [{tableName}] ({string.Join(",", columnNames.Select(c => $"[{c}] VARCHAR").ToArray())});";
                    command.ExecuteNonQuery();
                    foreach (DataRow row in dataTable.Rows)
                    {
                        var rowValues = (from DataColumn column in dataTable.Columns select (row[column] != null && row[column] != DBNull.Value) ? row[column].ToString() : string.Empty).ToList();
                        rowValues.RemoveAt(1);
                        command.CommandText = $"INSERT INTO [{tableName}]({string.Join(",", columnNames.Select(c => $"[{c}]"))}) VALUES ({string.Join(",", rowValues.Select(r => $"'{r}'").ToArray())});";

                        command.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }

        private void studentsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
