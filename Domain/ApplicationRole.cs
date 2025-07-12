using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public string Description { get; set; } = null!;
    }
}
