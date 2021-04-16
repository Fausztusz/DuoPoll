using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DuoPoll.Dal.Entities
{
    public partial class User : IdentityUser<int>
    {
        public User()
        {
            Polls = new HashSet<Poll>();
            Choices = new HashSet<Choice>();
        }

        [InverseProperty("User")] public virtual ICollection<Poll> Polls { get; set; }
        [InverseProperty("User")] public virtual ICollection<Choice> Choices { get; set; }
    }
}