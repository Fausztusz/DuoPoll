using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace DuoPoll.Dal.Entities
{
    public partial class User:IdentityUser<int>
    {
        public User()
        {
            Polls = new HashSet<Poll>();
            Choices = new HashSet<Choice>();
        }

        public string Name { get; set; }

        public virtual ICollection<Poll> Polls { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }

    }
}