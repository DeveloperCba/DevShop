using DevShop.Core.Datas.Implementations;
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.DomainObjects;
using DevShop.Identity.Infrastructure.Context;

namespace DevShop.Identity.Infrastructure.Repository;

public class LogErrorRepository : Repository<LogError>, ILogErrorRepository
{
    public LogErrorRepository(LogDbContext context) : base(context)
    {

    }
}