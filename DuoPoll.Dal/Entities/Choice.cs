using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuoPoll.Dal.Entities
{
    public class Choice
    {
        public Choice()
        {
            // Answer = new Answer();
            // User = new User();
            // Loser = new Answer();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int AnswerId { get; set; }
        public int LoserId { get; set; }
        public int UserId { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual Answer Loser { get; set; }

        [InverseProperty("Choices")] public virtual User User { get; set; }
    }
}