using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamLeaderWpf.Core
{
    public class Report
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public ObjectId GroupId { get; set; }
        public DateOnly CurrentDate { get; set; }
        public List<Record> Records { get; set; }
    }
}
