using Microsoft.AspNetCore.Identity;

namespace JobApplication.DataModel.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
