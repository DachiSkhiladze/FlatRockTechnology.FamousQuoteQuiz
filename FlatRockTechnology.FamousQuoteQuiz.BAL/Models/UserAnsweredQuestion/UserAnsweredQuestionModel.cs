namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Models
{
    public class UserAnsweredQuestionModel
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public long QuestionID { get; set; }
        public long AnswerID { get; set; }
    }
}
