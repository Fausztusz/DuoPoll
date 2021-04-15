namespace DuoPoll.Dal.Entities
{
    public class Choice
    {
        public Choice()
        {
            Answer = new Answer();
            User = new User();
            Loser = new Answer();
        }

        public uint Id { get; set; }
        public uint AnswerId { get; set; }
        public uint LoserId { get; set; }
        public uint UserId { get; set; }

        public virtual Answer Answer { get; set; }
        public virtual User User { get; set; }
        public virtual Answer Loser { get; set; }
    }
}