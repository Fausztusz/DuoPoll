using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DuoPoll.Dal.Entities
{
    [Index(nameof(Id), nameof(Url))]
    public class Poll
    {
        public Poll()
        {
            // User = new User();
            // Answers = new HashSet<Answer>();
        }

        public enum StatusType
        {
            Open,
            Close,
            Draft,
            Locked,
            Expired
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(50)] [Required] public string Name { get; set; }
        [StringLength(32)] public string Url { get; set; } = Guid.NewGuid().ToString("n");

        public bool Public { get; set; }
        public StatusType Status { get; set; }
        public DateTime Open { get; set; } = DateTime.Now;
        public DateTime Close { get; set; } = DateTime.Now.AddDays(14);

        public virtual ICollection<Answer> Answers { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}