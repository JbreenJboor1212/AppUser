using Microsoft.AspNetCore.Identity;

namespace AppUser.Models
{
    public class AppUserT : IdentityUser
    {
        public ICollection<Portfolio> Portfolios { get; set; } = new List<Portfolio>();
    }
}
