using Microsoft.AspNetCore.Identity;

namespace DevShop.Identity.Domain.Models;

public class ApplicationUserToken : IdentityUserToken<string>
{
    public virtual ApplicationUser User { get; set; }
}