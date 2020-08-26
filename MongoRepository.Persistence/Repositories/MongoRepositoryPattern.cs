using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Serilog;

using MongoDB.Bson;
using MongoDB.Driver;

using MongoRepository.Domain.Entities;
using MongoRepository.Domain.Repositories;

namespace MongoRepository.Persistence.Repositories
{
    public class MongoRepositoryPattern : IRepository
    {
        private IMongoDatabase MongoDatabase;

        public MongoRepositoryPattern(MongoDataContext context)
        {
            MongoDatabase = context.MongoDatabase;

            Log.Logger = new LoggerConfiguration()
                //.MinimumLevel.Debug()
                //.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                //.Enrich.FromLogContext()
                //.WriteTo.File("log.txt", rollingInterval: RollingInterval.Day)

                .WriteTo.MongoDB($"mongodb://{MongoDatabase.Client.Settings.Server.ToString()}/{context.DbName}-log", collectionName: "Log")
                .CreateLogger();
        }

        #region CollectionName
        public virtual string CollectionName<Model>() where Model : Base, new()
        {
            try
            {
                return typeof(Model).Name;
            }
            catch (Exception ex)
            {
                Log.Logger?.Fatal(ex.Message);
                throw;
            }
        }
        public virtual IMongoCollection<Model> Collection<Model>(string collectionName = null) where Model : Base, new()
        {
            try
            {
                return MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName);
            }
            catch (Exception ex)
            {
                Log.Logger?.Fatal(ex.Message);
                throw;
            }
        }
        #endregion

        #region async

        public virtual async Task<ISingleResult<Model>> InsertOneAsync<Model>(Model model, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<Model>();

            try
            {
                if (!Guid.TryParse(model._id, out Guid NewGuid))
                    throw new ArgumentNullException("Model ID format is wrong");

                model.UpdatedAt = DateTime.Now;

                await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).InsertOneAsync(model);

                response.Model = model;
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<IListResult<Model>> InsertManyAsync<Model>(List<Model> models, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                foreach (var model in models)
                    if (!Guid.TryParse(model._id, out Guid NewGuid))
                        throw new ArgumentNullException("Model ID format is wrong");

                models.ForEach(x => x.UpdatedAt = DateTime.Now);

                await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).InsertManyAsync(models);

                response.Model = models;
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual async Task<ISingleResult<long>> CountDocumentsAsync<Model>(string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<long>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).CountDocumentsAsync(new BsonDocument());
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<ISingleResult<long>> CountDocumentsAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<long>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).CountDocumentsAsync(predicate);
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual async Task<ISingleResult<DeleteResult>> DeleteOneAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<DeleteResult>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).DeleteOneAsync(predicate);
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<ISingleResult<DeleteResult>> DeleteManyAsync<Model>(string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<DeleteResult>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).DeleteManyAsync(new BsonDocument());
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<ISingleResult<DeleteResult>> DeleteManyAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<DeleteResult>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).DeleteManyAsync(predicate);
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual async Task<ISingleResult<bool>> ExistAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<bool>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).AnyAsync();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<ISingleResult<Model>> GetAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<Model>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).FirstOrDefaultAsync<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual async Task<IListResult<Model>> GetListAsync<Model>(string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(new BsonDocument()).ToListAsync<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).ToListAsync<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, int limit, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Limit(limit).ToListAsync<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                SortDefinition<Model> sort = descending ? Builders<Model>.Sort.Descending(sortField) : Builders<Model>.Sort.Ascending(sortField);
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Sort(sort).ToListAsync<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                SortDefinition<Model> sort = descending ? Builders<Model>.Sort.Descending(sortField) : Builders<Model>.Sort.Ascending(sortField);
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Sort(sort).Limit(limit).ToListAsync<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, int skip, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                SortDefinition<Model> sort = descending ? Builders<Model>.Sort.Descending(sortField) : Builders<Model>.Sort.Ascending(sortField);
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Sort(sort).Skip(skip).Limit(limit).ToListAsync<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        [Obsolete]
        public virtual async Task<ISingleResult<string>> IndexesCreateOneAsync<Model>(string text, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<string>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<BsonDocument>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Indexes.CreateOneAsync(new BsonDocument(text, 1));
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<ISingleResult<Model>> GetSingleAsync<Model>(string id, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<Model>();

            try
            {
                response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(x => x._id.Equals(id)).FirstOrDefaultAsync();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        [Obsolete]
        public virtual async Task<ISingleResult<Model>> SaveAsync<Model>(Model model, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<Model>();

            try
            {
                if (!Guid.TryParse(model._id, out Guid NewGuid))
                    throw new ArgumentNullException("Model ID format is wrong");

                if (string.IsNullOrWhiteSpace(model._id))
                    model._id = Guid.NewGuid().ToString("N");

                model.UpdatedAt = DateTime.Now;

                await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).ReplaceOneAsync(
                    c => c._id.Equals(model._id),
                    model,
                    new UpdateOptions
                    {
                        IsUpsert = true
                    });

                response.Model = model;
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual async Task<ISingleResult<UpdateResult>> UpdateAsync<Model>(FilterDefinition<Model> filter, UpdateDefinition<Model> update, bool multi = false, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<UpdateResult>();

            try
            {
                update.Set(x => x.UpdatedAt, DateTime.Now);

                if (multi)
                {
                    response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).UpdateManyAsync(
                        filter,
                        update,
                        new UpdateOptions
                        {
                            IsUpsert = true
                        });
                }
                else
                {
                    response.Model = await MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).UpdateOneAsync(
                        filter,
                        update,
                        new UpdateOptions
                        {
                            IsUpsert = true
                        });
                }
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        #endregion

        #region NOT async

        public virtual ISingleResult<Model> InsertOne<Model>(Model model, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<Model>();

            try
            {
                if (!Guid.TryParse(model._id, out Guid NewGuid))
                    throw new ArgumentNullException("Model ID format is wrong");

                model.UpdatedAt = DateTime.Now;

                MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).InsertOne(model);

                response.Model = model;
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual IListResult<Model> InsertMany<Model>(List<Model> models, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                foreach (var model in models)
                    if (!Guid.TryParse(model._id, out Guid NewGuid))
                        throw new ArgumentNullException("Model ID format is wrong");

                models.ForEach(x => x.UpdatedAt = DateTime.Now);

                MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).InsertMany(models);

                response.Model = models;
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual ISingleResult<long> CountDocuments<Model>(string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<long>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).CountDocuments(new BsonDocument());
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual ISingleResult<long> CountDocuments<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<long>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).CountDocuments(predicate);
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual ISingleResult<DeleteResult> DeleteOne<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<DeleteResult>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).DeleteOne(predicate);
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual ISingleResult<DeleteResult> DeleteMany<Model>(string collectionName) where Model : Base, new()
        {
            var response = new SingleResult<DeleteResult>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).DeleteMany(new BsonDocument());
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual ISingleResult<DeleteResult> DeleteMany<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<DeleteResult>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).DeleteMany(predicate);
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual ISingleResult<bool> Exist<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<bool>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Any();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual ISingleResult<Model> Get<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<Model>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).FirstOrDefault<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        public virtual IListResult<Model> GetList<Model>(string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(new BsonDocument()).ToList<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).ToList<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, int limit, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Limit(limit).ToList<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                SortDefinition<Model> sort = descending ? Builders<Model>.Sort.Descending(sortField) : Builders<Model>.Sort.Ascending(sortField);
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Sort(sort).ToList<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                SortDefinition<Model> sort = descending ? Builders<Model>.Sort.Descending(sortField): Builders<Model>.Sort.Ascending(sortField);
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Sort(sort).Limit(limit).ToList<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, int skip, string collectionName = null) where Model : Base, new()
        {
            var response = new ListResult<Model>();

            try
            {
                SortDefinition<Model> sort = descending ? Builders<Model>.Sort.Descending(sortField) : Builders<Model>.Sort.Ascending(sortField);
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(predicate).Sort(sort).Skip(skip).Limit(limit).ToList<Model>();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        [Obsolete]
        public virtual ISingleResult<string> IndexesCreateOne<Model>(string text, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<string>();

            try
            {
                response.Model = MongoDatabase.GetCollection<BsonDocument>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Indexes.CreateOne(new BsonDocument(text, 1));
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual ISingleResult<Model> GetSingle<Model>(string id, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<Model>();

            try
            {
                response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).Find(x => x._id.Equals(id)).FirstOrDefault();
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        [Obsolete]
        public virtual ISingleResult<Model> Save<Model>(Model model, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<Model>();

            try
            {
                if (!Guid.TryParse(model._id, out Guid NewGuid))
                    throw new ArgumentNullException("Model ID format is wrong");

                if (string.IsNullOrWhiteSpace(model._id))                
                    model._id = Guid.NewGuid().ToString("N");
                
                model.UpdatedAt = DateTime.Now;

                MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).ReplaceOne(
                    c => c._id.Equals(model._id),
                    model,
                    new UpdateOptions
                    {
                        IsUpsert = true
                    });

                response.Model = model;
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }
        public virtual ISingleResult<UpdateResult> Update<Model>(FilterDefinition<Model> filter, UpdateDefinition<Model> update, bool multi = false, string collectionName = null) where Model : Base, new()
        {
            var response = new SingleResult<UpdateResult>();

            try
            {
                update.Set(x => x.UpdatedAt, DateTime.Now);

                if (multi)
                {
                    response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).UpdateMany(
                        filter,
                        update,
                        new UpdateOptions
                        {
                            IsUpsert = true
                        });
                }
                else
                {
                    response.Model = MongoDatabase.GetCollection<Model>(String.IsNullOrEmpty(collectionName) ? CollectionName<Model>() : collectionName).UpdateOne(
                        filter,
                        update,
                        new UpdateOptions
                        {
                            IsUpsert = true
                        });
                }
            }
            catch (MongoException ex)
            {
                response.HasError = true;
                response.Message = "Error occurred while MongoDB Query";

                Log.Logger?.Fatal(ex.Message);
            }
            catch (Exception ex)
            {
                response.HasError = true;
                response.Message = $"MongoRepository Exception: {ex.Message}";

                Log.Logger?.Fatal(ex.Message);
            }

            return response;
        }

        #endregion
    }
}