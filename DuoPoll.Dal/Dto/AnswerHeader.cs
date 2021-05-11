using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using DuoPoll.Dal.Entities;

namespace DuoPoll.Dal.Dto
{
    public class AnswerHeader
    {
        public int Id { get; set; }
        [NotNull]
        public string Title { get; set; }
        [AllowNull]
        public string Media { get; set; }
        public string Url { get; set; }
    }
}