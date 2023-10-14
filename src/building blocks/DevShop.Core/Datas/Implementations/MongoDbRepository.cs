using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DevShop.Core.Comunications;
using DevShop.Core.Datas.Helpers;
using DevShop.Core.Datas.Interfaces;
using DevShop.Core.Datas.Models;

namespace DevShop.Core.Datas.Implementations;

public class MongoDbRepository<TDocument> : IMongoDbRepository<TDocument>
    where TDocument : EntityDocument
{
    private readonly IMongoCollection<TDocument> _collection;
    private readonly MongoDbSettings _mongoDbSettings;
    public MongoDbRepository(IOptions<MongoDbSettings> mongoDbSettings)
    {
        _mongoDbSettings = mongoDbSettings.Value;
        var database = new MongoClient(_mongoDbSettings.ConnectionStrings).GetDatabase(_mongoDbSettings.DatabaseName);
        _collection = database.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
    }

    protected string GetCollectionName(Type documentType)
    {
        return ((BsonCollectionAttribute)documentType.GetCustomAttributes(
                typeof(BsonCollectionAttribute),
                true)
            .FirstOrDefault())?.CollectionName;
    }

    public virtual IQueryable<TDocument> AsQueryable()
    {
        return _collection.AsQueryable();
    }

    public virtual IEnumerable<TDocument> FilterBy(
        Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).ToEnumerable();
    }

    public virtual IEnumerable<TProjected> FilterBy<TProjected>(
        Expression<Func<TDocument, bool>> filterExpression,
        Expression<Func<TDocument, TProjected>> projectionExpression)
    {
        return _collection.Find(filterExpression).Project(projectionExpression).ToEnumerable();
    }

    public virtual TDocument FindOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.Find(filterExpression).FirstOrDefault();
    }

    public virtual async Task<TDocument> FindOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return await _collection.Find(filterExpression).FirstOrDefaultAsync();
    }

    public virtual TDocument FindById(string id)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
        return _collection.Find(filter).SingleOrDefault();
    }

    public virtual async Task<TDocument> FindByIdAsync(string id)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
        return await _collection.Find(filter).SingleOrDefaultAsync();
    }


    public virtual void InsertOne(TDocument document)
    {
        _collection.InsertOne(document);
    }

    public virtual async Task InsertOneAsync(TDocument document)
    {
        await _collection.InsertOneAsync(document);
    }

    public void InsertMany(ICollection<TDocument> documents)
    {
        _collection.InsertMany(documents);
    }


    public virtual async Task InsertManyAsync(ICollection<TDocument> documents)
    {
        await _collection.InsertManyAsync(documents);
    }

    public void ReplaceOne(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        _collection.FindOneAndReplace(filter, document);
    }

    public virtual async Task ReplaceOneAsync(TDocument document)
    {
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, document.Id);
        await _collection.FindOneAndReplaceAsync(filter, document);
    }

    public void DeleteOne(Expression<Func<TDocument, bool>> filterExpression)
    {
        _collection.FindOneAndDelete(filterExpression);
    }

    public async Task DeleteOneAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        await _collection.FindOneAndDeleteAsync(filterExpression);
    }

    public void DeleteById(string id)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
        _collection.FindOneAndDelete(filter);
    }

    public async Task DeleteByIdAsync(string id)
    {
        var objectId = new ObjectId(id);
        var filter = Builders<TDocument>.Filter.Eq(doc => doc.Id, objectId);
        await _collection.FindOneAndDeleteAsync(filter);
    }

    public bool DeleteMany(Expression<Func<TDocument, bool>> filterExpression)
    {
        return _collection.DeleteMany(filterExpression).DeletedCount > 0;
    }

    public async Task<bool> DeleteManyAsync(Expression<Func<TDocument, bool>> filterExpression)
    {
        return (await _collection.DeleteManyAsync(filterExpression)).DeletedCount > 0;
    }
}