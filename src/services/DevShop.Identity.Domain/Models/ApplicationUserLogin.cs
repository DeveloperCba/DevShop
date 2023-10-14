using Microsoft.AspNetCore.Identity;

namespace DevShop.Identity.Domain.Models;

public class ApplicationUserLogin : IdentityUserLogin<string>
{
    public virtual ApplicationUser User { get; set; }
}