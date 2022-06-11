namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Models
{
    public class UserQuestionHistory
    {
        public string QuestionText { get; set; }
        public string AnswerText { get; set; }
        public bool WasCorrect { get; set; }
        public QuestionModel Question { get; set; }
        public IEnumerable<AnswerModel> Answers { get; set; }
        public UserAnsweredQuestionModel? UserAnsweredQuestion{ get; set; }
    }
}
