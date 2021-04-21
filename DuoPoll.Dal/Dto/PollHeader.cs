using System;
using System.Collections.Generic;
using DuoPoll.Dal.Entities;

namespace DuoPoll.Dal.Dto
{
    public class PollHeader
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public Poll.StatusType Status { get; set; }
        public bool Public { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }

        public UserHeader User { get; set; }
        public List<AnswerHeader> Answers { get; set; }
    }
}