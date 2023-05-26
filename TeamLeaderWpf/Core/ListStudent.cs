using DocumentFormat.OpenXml.Wordprocessing;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamLeaderWpf.Core
{
    public class ListStudent : Student
    {
        public ListStudent(ObjectId id, bool isDuty, string firstName, string lastName, string middleName, char gender, DateTime birthDate,
            string birthPlace, string postCode, string districtByRegistartion, string registeredAddress, 
            string residenceAddress, string citizenship, string passportData, string itn, string inlila, string medicalPolicy,
            string phoneNumber, string fatherFullName, string fatherPhoneNumber, string motherFullName, string motherPhoneNumber)
            : base(firstName, lastName, middleName, gender, birthDate, birthPlace, postCode, districtByRegistartion, registeredAddress,
                residenceAddress, citizenship, passportData, itn, inlila, medicalPolicy, phoneNumber, fatherFullName,
                fatherPhoneNumber, motherFullName, motherPhoneNumber)
        {
            Id = id;
            IsDuty = isDuty;
        }

        public ListStudent(Student student, bool isDuty)
        {
            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            MiddleName = student.MiddleName;
            Gender = student.Gender;
            BirthDate = student.BirthDate;
            BirthPlace = student.BirthPlace;
            PostCode = student.PostCode;
            DistrictByRegistartion = student.DistrictByRegistartion;
            RegisteredAddress = student.RegisteredAddress;
            ResidenceAddress = student.ResidenceAddress;
            Citizenship = student.Citizenship;
            PassportData = student.PassportData;
            ITN = student.ITN;
            INLILA = student.INLILA;
            MedicalPolicy = student.MedicalPolicy;
            PhoneNumber = student.PhoneNumber;
            FatherFullName = student.FatherFullName;
            FatherPhoneNumber = student.FatherPhoneNumber;
            MotherFullName = student.MotherFullName;
            MotherPhoneNumber = student.MotherPhoneNumber;
            IsDuty = isDuty;
        }

        private bool _isDuty;
        public bool IsDuty 
        { 
            get { return _isDuty; }
            set
            {
                if (value == true)
                    DutyString = "  --  Дежурный";
                else
                    DutyString = "";
                _isDuty = value;
            }
        }
        public string DutyString { get; set; }
    }
}
