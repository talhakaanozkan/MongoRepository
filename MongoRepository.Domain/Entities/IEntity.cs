using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MongoRepository.Domain.Entities
{
    public interface IEntity<TKey, TStatus, TDateTime>
    {
        TKey _id { get; set; }
        TStatus Status { get; set; }
        TDateTime CreatedAt { get; set; }
        TDateTime UpdatedAt { get; set; }
    }

    public abstract class Base : IEntity<string, Status, DateTime>
    {
        [BsonId]
        public string _id { get; set; } = Guid.NewGuid().ToString("N");
        public Status Status { get; set; } = Status.Passive;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }

    public enum Status
    {
        Passive, Active, Deleted
    }
}
