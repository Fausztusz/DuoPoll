using System;
using System.Collections.Generic;

namespace IHateThisSh_t.Dal.Entities
{
    public class Poll
    {
        public Poll()
        {
            User = new User();
            Answers = new HashSet<Answer>();
        }

        public uint Id { get; set; }
        public string Name { get; set; }
        public bool Public { get; set; }
        public string Url { get; set; }
        public enum Status
        {
            Open,
            Close,
            Draft,
            Locked,
            Expired
        }
        public DateTime Open{ get; set; }
        public DateTime Close{ get; set; }

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual User User { get; set; }
    }
}