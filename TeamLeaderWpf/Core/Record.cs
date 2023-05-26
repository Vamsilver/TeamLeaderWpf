using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamLeaderWpf.Core
{
    public class Record
    {
        public Record(Student student, int totalHours)
        {
            StudentId = student.Id;
            TotalHours = totalHours;
        }

        public Record(Student student)
        {
            StudentId = student.Id;
        }

        public Record(ObjectId studentId, int totalHours)
        {
            StudentId = studentId;
            TotalHours = totalHours;
        }

        public Record()
        {
        }

        public ObjectId StudentId { get; set; }
        
        private bool _firstLesson;
        
        public bool FirstLesson
        {
            get { return _firstLesson; }
            set
            {
                _firstLesson = value;
                if (_firstLesson)
                    TotalHours += 2;
                else
                    TotalHours -= 2;
            }
        }

        private bool _secondLesson;

        public bool SecondLesson
        {
            get { return _secondLesson; }
            set
            {
                _secondLesson = value;
                if (_secondLesson)
                    TotalHours += 2;
                else
                    TotalHours -= 2;
            }
        }

        private bool _thirdLesson;

        public bool ThirdLesson
        {
            get { return _thirdLesson; }
            set
            {
                _thirdLesson = value;
                if (_thirdLesson)
                    TotalHours += 2;
                else
                    TotalHours -= 2;
            }
        }

        private bool _fourthLesson;

        public bool FourthLesson
        {
            get { return _fourthLesson; }
            set
            {
                _fourthLesson = value;
                if (_fourthLesson)
                    TotalHours += 2;
                else
                    TotalHours -= 2;
            }
        }
        public int TotalHours { get; set; }
    }
}
