using System.ComponentModel.DataAnnotations;

namespace JobApplication.Application.DTOs
{
    public class CreateJobDTO
    {
        [Required]
        [StringLength(200)]
        public string title { get; set; }
        [Required]
        [StringLength(2000)]
        public string description { get; set; }
    }
}
