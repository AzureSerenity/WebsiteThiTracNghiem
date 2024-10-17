using Microsoft.AspNetCore.Identity;

namespace DoAnCoSo.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Base64EncodedPassword { get; set; }
        public string FullName { get; set; }
        public string? Age { get; set; }
    }
}
