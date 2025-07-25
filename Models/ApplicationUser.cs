using Microsoft.AspNetCore.Identity;

namespace CourseWebAPI.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
