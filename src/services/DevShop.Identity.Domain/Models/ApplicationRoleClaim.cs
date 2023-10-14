using Microsoft.AspNetCore.Identity;

namespace DevShop.Identity.Domain.Models;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public virtual ApplicationRole Role { get; set; }
}