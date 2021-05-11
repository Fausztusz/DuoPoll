using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using DuoPoll.Dal.Entities;
using Newtonsoft.Json;

namespace DuoPoll.Dal.Dto
{
    public class PollHeader
    {
        public int Id { get; set; }
        [AllowNull]
        [JsonIgnore]
        public int UserId { get; set; }

        [MaxLength(50)] [Required] public string Name { get; set; }
        [StringLength(32)] public string Url { get; set; }

        public bool Public { get; set; }
        public Poll.StatusType Status { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }

        [InverseProperty("Poll")]
        public virtual ICollection<Answer> Answers { get; set; }
    }
}