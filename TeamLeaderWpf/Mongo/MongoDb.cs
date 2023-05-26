using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeaderWpf.Core;

namespace TeamLeaderWpf.Mongo
{
    public class MongoDb
    {
        public static User? currentUser;
        public static Core.Group? currentGroup;

        public static void AddUserToDataBase(User user)
        {
            GetUsers().InsertOne(user);
        }

        public static void AddStudentToDataBase(Student student)
        {
            GetStudents().InsertOne(student);
        }

        public static void AddGroupToDataBase(Core.Group group)
        {
            GetGroups().InsertOne(group);
        }

        public static void AddReportToDataBase(Report report)
        {
            GetReports().InsertOne(report);
        }


        public static void ReplaceOneUser(User user)
        {
            var filter = new BsonDocument("_id", user.Id);
            GetUsers().ReplaceOne(filter, user);
        }

        public static void ReplaceOneReport(Report report)
        {
            var filter = new BsonDocument("_id", report.Id);
            GetReports().ReplaceOne(filter, report);
        }

        public static void ReplaceOneStudentInDataBase(TabStudent tabStudent)
        {
            tabStudent.BirthDate = tabStudent.BirthDate.AddDays(1); 
            Student newStudent = new Student()
            {
                Id = tabStudent.Id,
                FirstName = tabStudent.FirstName,
                LastName = tabStudent.LastName,
                MiddleName = tabStudent.MiddleName,
                Gender = tabStudent.Gender,
                BirthDate = tabStudent.BirthDate,
                BirthPlace = tabStudent.BirthPlace,
                PostCode = tabStudent.PostCode,
                DistrictByRegistartion = tabStudent.DistrictByRegistartion,
                RegisteredAddress = tabStudent.RegisteredAddress,
                ResidenceAddress = tabStudent.ResidenceAddress,
                Citizenship = tabStudent.Citizenship,
                PassportData = tabStudent.PassportData,
                ITN = tabStudent.ITN,
                INLILA= tabStudent.INLILA,
                MedicalPolicy = tabStudent.MedicalPolicy,
                PhoneNumber = tabStudent.PhoneNumber,
                FatherFullName = tabStudent.FatherFullName,
                FatherPhoneNumber = tabStudent.FatherPhoneNumber,
                MotherFullName = tabStudent.MotherFullName,
                MotherPhoneNumber = tabStudent.MotherPhoneNumber
            };
            var filter = new BsonDocument("_id", tabStudent.Id);
            GetStudents().ReplaceOne(filter, newStudent);
        }

        public static User FindUserById(string id)
        {
            var filter = new BsonDocument("_id", ObjectId.Parse(id));
            return GetUsers().Find(filter).FirstOrDefault();
        }

        public static User FindUserByLogin(string login)
        {
            var filter = new BsonDocument("Login", login);
            return GetUsers().Find(filter).FirstOrDefault();
        }
        public static User FindUserByEmail(string email)
        {
            var filter = new BsonDocument("Email", email);
            return GetUsers().Find(filter).FirstOrDefault();
        }

        public static Core.Group FindGroupByName(string GroupName)
        {
            var filter = new BsonDocument("Name", GroupName);
            return GetGroups().Find(filter).FirstOrDefault();
        }

        public static Core.Group FindGroupById(string id)
        {
            var filter = new BsonDocument("_id", ObjectId.Parse(id));
            return GetGroups().Find(filter).FirstOrDefault();
        }

        public static Core.Group FindGroupById(ObjectId id)
        {
            var filter = new BsonDocument("_id", id);
            return GetGroups().Find(filter).FirstOrDefault();
        }

        public static Student FindStudentByFirstName(string firstName)
        {
            var filter = new BsonDocument("FirstName", firstName);
            return GetStudents().Find(filter).FirstOrDefault();
        }

        public static Student FindStudentById(string id)
        {
            var filter = new BsonDocument("_id", ObjectId.Parse(id));
            return GetStudents().Find(filter).FirstOrDefault();
        }

        public static Student FindStudentById(ObjectId id)
        {
            var filter = new BsonDocument("_id", id);
            return GetStudents().Find(filter).FirstOrDefault();
        }

        public static List<Student> FindStudentsByIdList(List<string> ids)
        {
            List<Student> answer = new List<Student>();
            foreach(var id in ids)
            {
                var filter = new BsonDocument("_id", ObjectId.Parse(id));
                var student = GetStudents().Find(filter).FirstOrDefault();

                if (student != null)
                    answer.Add(student);
                else
                    continue;
            }

            return answer;
        }

        public static List<Student> GetAllStudentsInCurrentGroup()
        {
            var filter = new BsonDocument("Name", currentGroup?.Name);
            var group = GetGroups().Find(filter).FirstOrDefault();
            List<Student> students = new List<Student>();
            foreach(var st in group.StudentsId)
            {
                students.Add(FindStudentById(st));
            }

            return students;
        }

        public static void DeleteOneReport(Report report)
        {
            var filter = new BsonDocument("_id", report.Id);
            GetReports().DeleteOne(filter); 
        }

        public static List<Report> GetAllReportsInCurrentGroup()
        {
            var filter = new BsonDocument("GroupId", currentGroup?.Id);
            return GetReports().Find(filter).ToList();
        }

        public static IMongoCollection<User> GetUsers()
        {
            return GetDatabase().GetCollection<User>("UsersCollection");
        }

        public static IMongoCollection<Student> GetStudents()
        {
            return GetDatabase().GetCollection<Student>("StudentsCollection");
        }

        public static IMongoCollection<Core.Group> GetGroups()
        {
            return GetDatabase().GetCollection<Core.Group>("GroupsCollection");
        }

        public static IMongoCollection<Report> GetReports()
        {
            return GetDatabase().GetCollection<Report>("ReportsCollection");
        }

        private static IMongoDatabase GetDatabase()
        {
            var client = new MongoClient("mongodb://localhost");
            return client.GetDatabase("TeamLeader");
        }
    }
}
