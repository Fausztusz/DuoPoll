using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuoPoll.Dal.Entities
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
        public string Url { get; set; } = "~/Polls/" + Guid.NewGuid().ToString("n").Substring(0, 12);

        [System.ComponentModel.DefaultValue(false)]
        public bool Public { get; set; }

        [System.ComponentModel.DefaultValue("Draft")]
        public enum Status
        {
            Open,
            Close,
            Draft,
            Locked,
            Expired
        }

        public DateTime Open { get; set; } = DateTime.Now;
        public DateTime Close { get; set; } = DateTime.Now.AddDays(14);

        public virtual ICollection<Answer> Answers { get; set; }
        public virtual User User { get; set; }
    }
}