using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.HelpAndAnswer
{
    public class HelpCategoryRequest
    {
        [Required(ErrorMessage = "Help category name is required")]
        public string HelpCategoryName { get; set; }
    }
}
