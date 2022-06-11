using FlatRockTechnology.FamousQuoteQuiz.BAL.Models.Answer;

namespace FlatRockTechnology.FamousQuoteQuiz.BAL.Models
{
    public class QuestionModel
    {
        public long ID { get; set; }
        public string? Text { get; set; }
        public string? AuthorName { get; set; }
        public bool? IsBinary { get; set; }
        public IEnumerable<AnswerForUserModel> Answers { get; set; }
    }
}
