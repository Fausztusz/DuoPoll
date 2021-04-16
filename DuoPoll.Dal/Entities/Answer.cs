using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace DuoPoll.Dal.Entities
{
    public class Answer
    {
        public Answer()
        {
            Poll = new Poll();
            Choices = new HashSet<Choice>();
            Losses = new HashSet<Choice>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int PollId { get; set; }
        [MaxLength(50)] public string Title { get; set; }

        [MaxLength(255)] [AllowNull] public string Media { get; set; }

        [InverseProperty("Answers")] public virtual Poll Poll { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
        public virtual ICollection<Choice> Losses { get; set; }
    }
}