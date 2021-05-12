using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace DuoPoll.Dal.Entities
{
    [Index(nameof(Id), nameof(UserIdentity))]
    public class Choice
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] public int AnswerId { get; set; }
        [Required] public int LoserId { get; set; }
        [AllowNull] public string UserIdentity { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Answer Loser { get; set; }
    }
}