using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Audio
{
    public class AudioCategoryRequest
    {
        [Required(ErrorMessage = "Audio category name is required")]
        public string AudioCategoryName { get; set; }
    }
}
