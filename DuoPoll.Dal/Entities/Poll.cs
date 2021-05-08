using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        public static string GetStatusString()
        {
            return $"'{StatusType.Open.ToString()}','{StatusType.Close.ToString()}','{StatusType.Draft.ToString()}','{StatusType.Locked.ToString()}','{StatusType.Expired.ToString()}'";
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
        [AllowNull] // Required in DbContext
        public int UserId { get; set; }

        [MaxLength(50)] [Required] public string Name { get; set; }
        [StringLength(32)] public string Url { get; set; } = Guid.NewGuid().ToString("n");

        public bool Public { get; set; }
        public StatusType Status { get; set; }
        public DateTime Open { get; set; } = DateTime.Now;
        public DateTime Close { get; set; } = DateTime.Now.AddDays(14);

        [InverseProperty("Poll")]
        public virtual ICollection<Answer> Answers { get; set; }

        [JsonIgnore]
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}