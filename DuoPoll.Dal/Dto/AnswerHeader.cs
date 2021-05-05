using System;
using System.Collections.Generic;
using DuoPoll.Dal.Entities;

namespace DuoPoll.Dal.Dto
{
    public class AnswerHeader
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Media { get; set; }
    }

    public class AnswerChoiceHeader
    {
        public List<ChoiceHeader> Choices { get; set; }
        public List<ChoiceHeader> Losses { get; set; }
    }
}