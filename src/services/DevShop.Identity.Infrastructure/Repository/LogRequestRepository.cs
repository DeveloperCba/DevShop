using DevShop.Core.Datas.Implementations;
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.DomainObjects;
using DevShop.Identity.Infrastructure.Context;

namespace DevShop.Identity.Infrastructure.Repository;

public class LogRequestRepository : Repository<LogRequest>, ILogRequestRepository
{
    private readonly LogDbContext _context;
    public LogRequestRepository(LogDbContext context) : base(context)
    {
        _context = context;
    }
    public IUnitOfWork UnitOfWork => _context;
}