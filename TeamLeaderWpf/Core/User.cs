using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamLeaderWpf.Core
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string GroupId { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string GoogleFolder { get; set; }
        public string GoogleSummaryPage { get; set; }
        public string GoogleFormSix { get; set; }

        public User(string login, string password, string email, string groupId)
        {
            Login = login;
            GroupId = groupId;
            Email = email;
            Password = password;
        }
    }
}
