using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract.Dtos.Requests.Quizz
{
    public class QuizzCategoryUpdateRequest
    {

        public string? QuizCategoryName { get; set; }

        public string? QuizCategoryDescription { get; set; }
    }
}
