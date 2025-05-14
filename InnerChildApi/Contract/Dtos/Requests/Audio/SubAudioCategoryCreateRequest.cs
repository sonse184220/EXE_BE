using System.ComponentModel.DataAnnotations;

namespace Contract.Dtos.Requests.Audio
{
    public class SubAudioCategoryCreateRequest
    {
        [Required(ErrorMessage ="Sub audio category name is required")]
        public string SubAudioCategoryName { get; set; }
        [Required(ErrorMessage ="Audio category id is required")]
        public string AudioCategoryId { get; set; }

    }
}
