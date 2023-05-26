using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamLeaderWpf.Core
{
    public class TabStudent : Student
    {
        public TabStudent(ObjectId id, int orderNumber, string firstName, string lastName, string middleName, char gender, DateTime birthDate,
            string birthPlace, string postCode, string districtByRegistartion, string registeredAddress, 
            string residenceAddress, string citizenship, string passportData, string itn, string inlila, string medicalPolicy,
            string phoneNumber, string fatherFullName, string fatherPhoneNumber, string motherFullName, string motherPhoneNumber)
            : base(firstName, lastName, middleName, gender, birthDate, birthPlace, postCode, districtByRegistartion, registeredAddress,
                residenceAddress, citizenship, passportData, itn, inlila, medicalPolicy, phoneNumber, fatherFullName,
                fatherPhoneNumber, motherFullName, motherPhoneNumber)
        {
            Id = id;
            OrderNumber = orderNumber;
        }

        public int OrderNumber { get; set; }
    }
}
