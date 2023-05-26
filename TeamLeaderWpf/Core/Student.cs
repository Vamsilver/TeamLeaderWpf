using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamLeaderWpf.Core
{
    public class Student
    {
        public Student(string firstName, string lastName, string middleName, char gender, DateTime birthDate,
            string birthPlace, string postCode, string districtByRegistartion, string registeredAddress, 
            string residenceAddress, string citizenship, string passportData, string itn, string inlila, string medicalPolicy,
            string phoneNumber, string fatherFullName, string fatherPhoneNumber, string motherFullName, string motherPhoneNumber)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
            Gender = gender;
            BirthDate = birthDate;
            BirthPlace = birthPlace;
            PostCode = postCode;
            DistrictByRegistartion = districtByRegistartion;
            RegisteredAddress = registeredAddress;
            ResidenceAddress = residenceAddress;
            Citizenship = citizenship;
            PassportData = passportData;
            ITN = itn;
            INLILA = inlila;
            MedicalPolicy = medicalPolicy;
            PhoneNumber = phoneNumber;
            FatherFullName = fatherFullName;
            FatherPhoneNumber = fatherPhoneNumber;
            MotherFullName = motherFullName;
            MotherPhoneNumber = motherPhoneNumber;
        }

        public Student(string firstName, string lastName, string middleName)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }

        public Student()
        {
        }

        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public Char Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string PostCode { get; set; }
        public string DistrictByRegistartion { get; set; }
        public string RegisteredAddress { get; set; }
        public string ResidenceAddress { get; set; }
        public string Citizenship { get; set; }
        public string PassportData { get; set; }
        public string ITN { get; set; }
        public string INLILA { get; set; }
        public string MedicalPolicy { get; set; }
        public string PhoneNumber { get; set; }
        public string FatherFullName { get; set; }
        public string FatherPhoneNumber { get; set; }
        public string MotherFullName { get; set; }
        public string MotherPhoneNumber { get; set; }

        public override string ToString()
        {
            return $"{LastName} {FirstName[0]}. {MiddleName[0]}.";
        }
    }
}
