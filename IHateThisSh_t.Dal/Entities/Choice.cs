namespace IHateThisSh_t.Dal.Entities
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
        public virtual Answer Answer { get; set; }
        public virtual User User { get; set; }
        public virtual Answer Loser { get; set; }
    }
}