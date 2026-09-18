using System.ComponentModel.DataAnnotations;

namespace JobApplication.Application.DTOs
{
    public class LogoutRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
