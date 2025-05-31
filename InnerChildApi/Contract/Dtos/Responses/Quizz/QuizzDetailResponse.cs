using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Responses.Quizz
{
    public class QuizzDetailResponse
    {
        public string QuizId { get; set; }

        public string QuizTitle { get; set; }

        public string QuizDescription { get; set; }

        public string QuizStatus { get; set; }

        public double? QuizMaxScore { get; set; }

        public string QuizCategoryId { get; set; }
        public List<QuizzQuestionDetailResponse> Questions { get; set; } = new();
    }
    public class QuizzQuestionDetailResponse
    {
        public string QuizQuestionId { get; set; }

        public string QuizQuestionContent { get; set; }

        public int? QuizQuestionOrder { get; set; }

        public string QuizId { get; set; }
        public List<QuizzOptionDetailResponse> Options { get; set; } = new();
    }
    public class QuizzOptionDetailResponse 
    {
        public string QuizAnswerOptionId { get; set; }

        public string QuizContent { get; set; }

        public double? QuizScoreValue { get; set; }

        public string QuestionId { get; set; }
    }
}
