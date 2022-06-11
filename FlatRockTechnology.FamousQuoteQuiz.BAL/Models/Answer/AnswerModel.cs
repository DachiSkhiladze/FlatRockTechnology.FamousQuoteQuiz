namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Models
{
    public class AnswerModel
    {
        public long ID { get; set; }
        public long QuestionID { get; set; }
        public string? Text { get; set; }
        public bool IsCorrect { get; set; }
    }
}
