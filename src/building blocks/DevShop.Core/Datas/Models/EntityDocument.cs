using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace DevShop.Core.Datas.Models;

public abstract class EntityDocument
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public ObjectId Id { get; set; }

    public EntityDocument()
    {
        Id = new ObjectId();
    }

    public DateTime CreatedAt => Id.CreationTime;
}