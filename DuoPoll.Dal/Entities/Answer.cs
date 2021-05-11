using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DuoPoll.Dal.Entities
{
    public class Answer
    {
        public Answer()
        {
            // Poll = new Poll();
            // Choices = new HashSet<Choice>();
            // Losses = new HashSet<Choice>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [JsonIgnore]
        public int PollId { get; set; }
        [MaxLength(50)][Required] public string Title { get; set; }

        [MaxLength(255)] [AllowNull] public string Media { get; set; }

        [JsonIgnore]
        [InverseProperty("Answers")]
        [ForeignKey("PollId")]
        public virtual Poll Poll { get; set; }

        public virtual ICollection<Choice> Choices { get; set; }
        public virtual ICollection<Choice> Losses { get; set; }
    }
}