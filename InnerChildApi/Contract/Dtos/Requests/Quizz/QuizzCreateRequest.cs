using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Quizz
{
    public class QuizzCreateRequest
    {
        public class CreateQuizzDto
        {
            //public string QuizId { get; set; }
            [Required(ErrorMessage = "Quizz title is required")]
            public string? QuizTitle { get; set; }
            [Required(ErrorMessage = "Quiz description is required")]
            public string? QuizDescription { get; set; }

            //public DateTime? QuizCreatedAt { get; set; }

            //public DateTime? QuizUpdatedAt { get; set; }

            //public QuizzEnum? QuizStatus { get; set; }
            [Required(ErrorMessage = "Quiz max score is required")]
            public double? QuizMaxScore { get; set; }
            [Required(ErrorMessage = "Quiz Category id is required")]
            public string QuizCategoryId { get; set; }

            public List<CreateQuizzQuestionDto> QuizzQuestions { get; set; }


        }

        public class CreateQuizzQuestionDto
        {
            //public string QuizQuestionId { get; set; }
            [Required(ErrorMessage = "Quizz question content is required")]
            public string QuizQuestionContent { get; set; }
            [Required(ErrorMessage = "Question order is required")]
            [Range(1, int.MaxValue, ErrorMessage = "Order must be at least 1")]
            public int? QuizQuestionOrder { get; set; }

            //public string QuizId { get; set; }
            public List<CreateQuizzOptionDto> QuizzOptions { get; set; }

        }
        public class CreateQuizzOptionDto
        {
            //public string QuizAnswerOptionId { get; set; }
            [Required(ErrorMessage = "Quiz content is required")]
            public string QuizContent { get; set; }
            [Required(ErrorMessage = "Option score value is required")]
            public double? QuizScoreValue { get; set; }

            //public string QuestionId { get; set; }

        }
        public class CreateQuizzCategoryDto
        {
            //public string QuizCategoryId { get; set; }
            [Required(ErrorMessage = "Quizz category name is required ")]
            public string QuizCategoryName { get; set; }
            [Required(ErrorMessage = "Quizz category description is required ")]

            public string QuizCategoryDescription { get; set; }
        }


    }
}
