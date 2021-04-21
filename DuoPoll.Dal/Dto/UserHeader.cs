using System.Collections.Generic;
using System.Linq;
using DuoPoll.Dal.Entities;

namespace DuoPoll.Dal.Dto
{
    public class UserHeader
    {
        public string UserName { get; set; }

        public List<Poll> Polls { get; set; }
        public List<Choice> Choices { get; set; }
    }
}