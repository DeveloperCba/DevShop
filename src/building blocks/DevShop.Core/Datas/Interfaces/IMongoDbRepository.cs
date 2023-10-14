using DevShop.Core.Datas.Models;
using System.Linq.Expressions;

namespace DevShop.Core.Datas.Interfaces;

public interface IMongoDbRepository<TDocument> where TDocument : EntityDocument
{
    IQueryable<TDocument> AsQueryable();

    IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression);

    IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression);

    TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression);

    Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    TDocument FindById(string id);

    Task<TDocument> FindByIdAsync(string id);

    void InsertOne(TDocument document);

    Task InsertOneAsync(TDocument document);

    void InsertMany(ICollection<TDocument> documents);

    Task InsertManyAsync(ICollection<TDocument> documents);

    void ReplaceOne(TDocument document);

    Task ReplaceOneAsync(TDocument document);

    void DeleteOne(Expression<Func<TDocument, bool>> filterExpression);

    Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression);

    void DeleteById(string id);

    Task DeleteByIdAsync(string id);

    bool DeleteMany(Expression<Func<TDocument, bool>> filterExpression);

    Task<bool> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression);
}