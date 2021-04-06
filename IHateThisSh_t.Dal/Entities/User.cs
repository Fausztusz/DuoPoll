using System.Collections.Generic;

namespace IHateThisSh_t.Dal.Entities
{
    public class User
    {
        public User()
        {
            Polls = new HashSet<Poll>();
            Choices = new HashSet<Choice>();
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Poll> Polls { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }

    }
}