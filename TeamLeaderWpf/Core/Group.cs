using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamLeaderWpf.Core
{
    public class Group
    {
        public Group(string name, List<string> studentsId)
        {
            Name = name;
            StudentsId = studentsId;
        }

        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public List<string> StudentsId{ get; set; }
    }
}
