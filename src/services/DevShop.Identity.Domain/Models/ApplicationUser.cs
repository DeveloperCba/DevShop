using Microsoft.AspNetCore.Identity;

namespace DevShop.Identity.Domain.Models;

public class ApplicationUser : IdentityUser
{
    public override string UserName { get; set; }
    public override string Email { get; set; }
    public string Name { get; set; }
    public string Document { get; set; }
    public string Street { get; set; }
    public string Complement { get; set; }
    public string Number { get; set; }
    public string ZipCode { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string State { get; set; }

 
    public virtual ICollection<ApplicationUserClaim> Claims { get; set; }
    public virtual ICollection<ApplicationUserLogin> Logins { get; set; }
    public virtual ICollection<ApplicationUserToken> Tokens { get; set; }
    public virtual ICollection<ApplicationUserRole> UserRoles { get; set; }
}