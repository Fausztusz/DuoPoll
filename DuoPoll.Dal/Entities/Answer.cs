using System.Collections.Generic;

namespace DuoPoll.Dal.Entities
{
    public class Answer
    {
        public Answer()
        {
            Poll = new Poll();
            Choices = new HashSet<Choice>();
        }

        public uint Id { get; set; }
        public string Title { get; set; }
        public string Media { get; set; }
        public virtual Poll Poll { get; set; }
        public virtual ICollection<Choice> Choices { get; set; }
    }
}