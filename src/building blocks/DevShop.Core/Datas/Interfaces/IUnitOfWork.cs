namespace DevShop.Core.Datas.Interfaces;

public interface IUnitOfWork
{
    Task<bool> Commit();
}