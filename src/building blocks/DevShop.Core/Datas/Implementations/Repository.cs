using Dapper;
using Dapper.Contrib.Extensions;
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.DomainObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Data;
using System.Linq.Expressions;

namespace DevShop.Core.Datas.Implementations;

public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, new()
{
    protected readonly IDbConnection Connection;
    protected readonly DbContext Context;
    internal DbSet<TEntity> dbSet;

    protected Repository(
        DbContext context
    )
    {
        Context = context;
        dbSet = Context.Set<TEntity>();
        Connection = Context.Database.GetDbConnection();
    }

    public async Task<TEntity> GetByIdAsync(int id) => await dbSet.FindAsync(id);
    public async Task<TEntity> GetByIdAsync(string id) => await dbSet.FindAsync(id);
    public async Task<TEntity> GetByIdAsync(Guid id) => await dbSet.FindAsync(id);
    public async Task<TEntity> GetByIdsAsync(object[] ids) => await dbSet.FindAsync(ids);

    public async Task<TEntity> GetByFilterAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Expression<Func<TEntity, TEntity>> field = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool isTracking = false)
    {
        IQueryable<TEntity> query = dbSet;
        try
        {
            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (field != null)
                query = query.Select(field);

            if (!isTracking)
                query = query.AsNoTracking();

            return await query.FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("GetByFilter: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return null;
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Expression<Func<TEntity, TEntity>> field = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool isTracking = false)
    {
        IQueryable<TEntity> query = dbSet;
        try
        {
            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (field != null)
                query = query.Select(field);

            if (orderBy != null)
                return orderBy(query);

            if (!isTracking)
                query = query.AsNoTracking();

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("GetAll: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return null;
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllPaginationAsync(
        Expression<Func<TEntity, bool>> filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
        Expression<Func<TEntity, TEntity>> field = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
        bool isTracking = false,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        IQueryable<TEntity> query = dbSet;
        try
        {
            if (filter != null)
                query = query.Where(filter);

            if (include != null)
                query = include(query);

            if (field != null)
                query = query.Select(field);

            if (orderBy != null)
                return orderBy(query);

            if (!isTracking)
                query = query.AsNoTracking();

            query = query.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);

            return await query.ToListAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("GetAll: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return null;
        }
    }

    public async Task<bool> HasValueAsync(Expression<Func<TEntity, bool>> filter = null)
    {
        try
        {
            if (filter == null)
                return (await dbSet.CountAsync()) > 0;
 
                return (await dbSet.Where(filter).CountAsync()) > 0;
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("HasValue: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return false;
        }
    }

    public async Task AddAsync(TEntity entity)
    {
        try
        {
            await dbSet.AddAsync(entity);
            //await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("Add: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
        }
    }

    public async Task AddRangerAsync(IEnumerable<TEntity> entities)
    {
        try
        {
            await dbSet.AddRangeAsync(entities);
            //await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("AddRanger: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
        }
    }

    public async Task UpdateAsync(TEntity entity)
    {
        try
        {
            dbSet.Update(entity);
            //await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("Update: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
        }
    }

    public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
    {
        try
        {
            dbSet.UpdateRange(entities);
            //await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("UpdateRange: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
        }
    }

    public async Task RemoveAsync(TEntity entity)
    {
        try
        {
            dbSet.Remove(entity);
            //await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("Remove: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
        }
    }

    public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
    {
        try
        {
            dbSet.RemoveRange(entities);
            //await SaveChangesAsync();
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("RemoveRangeAsync: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
        }
    }

    public async Task<int> SaveChangesAsync() => await Context.SaveChangesAsync();


    //Dapper
    public async Task<TEntity> ExecuteReturnScalerAsync(string procedureNameOrSql, DynamicParameters param = null, CommandType commandType = CommandType.Text)
    {
        try
        {
            return (TEntity)Convert.ChangeType(await Connection.ExecuteScalarAsync<TEntity>(procedureNameOrSql, param, commandType: commandType), typeof(TEntity));
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("ExecuteReturnScaler: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return default;
        }
    }

    public async Task ExecuteWithoutReturnAsync(string procedureNameOrSql, DynamicParameters param = null, CommandType commandType = CommandType.Text)
    {
        try
        {
            await Connection.ExecuteAsync(procedureNameOrSql, param, commandType: commandType);
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("ExecuteWithoutReturn: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
        }
    }

    public async Task<TEntity> GetOrDefaultAsync(string procedureNameOrSql, DynamicParameters param = null, CommandType commandType = CommandType.Text)
    {
        try
        {
            return (TEntity)Convert.ChangeType(await Connection.QueryFirstOrDefaultAsync<TEntity>(procedureNameOrSql, param, commandType: commandType), typeof(TEntity));
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("FirstOrDefaultAsync: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return default;
        }
    }

    public async Task<TEntity> GetAsync(string procedureNameOrSql, DynamicParameters param = null, CommandType commandType = CommandType.Text)
    {
        try
        {
            return (TEntity)Convert.ChangeType(await Connection.QueryAsync<TEntity>(procedureNameOrSql, param, commandType: commandType), typeof(TEntity));
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("Get: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return default;
        }
    }

    public async Task<TEntity> GetByIdAsync(object id)
    {
        try
        {
            return (TEntity)Convert.ChangeType(await Connection.GetAsync<TEntity>(id), typeof(TEntity));
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("GetById: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return default;
        }
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(string procedureNameOrSql, DynamicParameters param = null, CommandType commandType = CommandType.Text)
    {
        try
        {
            return await Connection.QueryAsync<TEntity>(procedureNameOrSql, param, commandType: commandType);
        }
        catch (Exception ex)
        {
            await Context.Set<LogError>().AddAsync(new LogError { Method = string.Concat("GetAll: ", GetType().Name), Path = typeof(TEntity).FullName, Erro = ex.Message, ErroCompleto = ex.ToString(), Query = dbSet.ToQueryString() });
            return default;
        }
    }


    public bool CheckConnection()
    {
        try
        {
            Connection.Open();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        Context?.Dispose();
        Connection?.Dispose();
    }
 
}