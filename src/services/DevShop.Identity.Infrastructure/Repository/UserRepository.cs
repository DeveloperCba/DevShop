using DevShop.Core.Datas.Implementations;
using DevShop.Identity.Domain.Interfaces;
using DevShop.Identity.Domain.Models;
using DevShop.Identity.Infrastructure.Context;

namespace DevShop.Identity.Infrastructure.Repository;

public class UserRepository : Repository<ApplicationUser>, IUserRepository
{
    public UserRepository(ApplicationIdentityDbContext context) : base(context)
    {
       
    }
}