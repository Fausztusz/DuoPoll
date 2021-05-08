using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace DuoPoll.Dal.Entities
{
    public partial class User : IdentityUser<int>
    {
        public User()
        {
            Polls = new HashSet<Poll>();
        }

        [AllowNull]
        [InverseProperty("User")] public virtual ICollection<Poll> Polls { get; set; }
    }
}