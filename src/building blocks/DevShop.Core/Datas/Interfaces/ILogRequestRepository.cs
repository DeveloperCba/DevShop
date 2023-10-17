using DevShop.Core.DomainObjects;

namespace DevShop.Core.Datas.Interfaces;

public interface ILogRequestRepository : IRepository<LogRequest>
{
    IUnitOfWork UnitOfWork { get; }
}