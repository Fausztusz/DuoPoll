namespace DuoPoll.Dal.Dto
{
    public class ChoiceHeader
    {
        public AnswerHeader Answer { get; set; }
        public AnswerHeader Loser { get; set; }
        public UserHeader User { get; set; }
    }
}