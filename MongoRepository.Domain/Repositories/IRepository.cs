using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using MongoRepository.Domain.Entities;

using MongoDB.Driver;

namespace MongoRepository.Domain.Repositories
{
    public interface IRepository
    {

        #region Collection
        string CollectionName<Model>() where Model : Base, new();
        IMongoCollection<Model> Collection<Model>(string collectionName = null) where Model : Base, new();
        #endregion

        #region With async
        Task<ISingleResult<Model>> InsertOneAsync<Model>(Model model, string collectionName = null) where Model : Base, new();
        Task<IListResult<Model>> InsertManyAsync<Model>(List<Model> model, string collectionName = null) where Model : Base, new();

        Task<ISingleResult<long>> CountDocumentsAsync<Model>(string collectionName = null) where Model : Base, new();
        Task<ISingleResult<long>> CountDocumentsAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();

        Task<ISingleResult<DeleteResult>> DeleteOneAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
        Task<ISingleResult<DeleteResult>> DeleteManyAsync<Model>(string collectionName = null) where Model : Base, new();
        Task<ISingleResult<DeleteResult>> DeleteManyAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();

        Task<ISingleResult<bool>> ExistAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName) where Model : Base, new();

        Task<ISingleResult<Model>> GetAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName) where Model : Base, new();

        Task<IListResult<Model>> GetListAsync<Model>(string collectionName = null) where Model : Base, new();
        Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
        Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, int limit, string collectionName = null) where Model : Base, new();
        Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, string collectionName = null) where Model : Base, new();
        Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, string collectionName = null) where Model : Base, new();
        Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, int skip, string collectionName = null) where Model : Base, new();

        Task<ISingleResult<string>> IndexesCreateOneAsync<Model>(string text, string collectionName = null) where Model : Base, new();
        Task<ISingleResult<Model>> GetSingleAsync<Model>(string id, string collectionName = null) where Model : Base, new();

        Task<ISingleResult<Model>> SaveAsync<Model>(Model model, string collectionName = null) where Model : Base, new();
        Task<ISingleResult<UpdateResult>> UpdateAsync<Model>(FilterDefinition<Model> filter, UpdateDefinition<Model> update, bool multi = false, string collectionName = null) where Model : Base, new();
        #endregion

        #region Without async
        ISingleResult<Model> InsertOne<Model>(Model model, string collectionName = null) where Model : Base, new();
        IListResult<Model> InsertMany<Model>(List<Model> model, string collectionName = null) where Model : Base, new();

        ISingleResult<long> CountDocuments<Model>(string collectionName = null) where Model : Base, new();
        ISingleResult<long> CountDocuments<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();

        ISingleResult<DeleteResult> DeleteOne<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
        ISingleResult<DeleteResult> DeleteMany<Model>(string collectionName = null) where Model : Base, new();
        ISingleResult<DeleteResult> DeleteMany<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();

        ISingleResult<bool> Exist<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();

        ISingleResult<Model> Get<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();

        IListResult<Model> GetList<Model>(string collectionName = null) where Model : Base, new();
        IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
        IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, int limit, string collectionName = null) where Model : Base, new();
        IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, string collectionName = null) where Model : Base, new();
        IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, string collectionName = null) where Model : Base, new();
        IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, int skip, string collectionName = null) where Model : Base, new();

        ISingleResult<string> IndexesCreateOne<Model>(string text, string collectionName = null) where Model : Base, new();
        ISingleResult<Model> GetSingle<Model>(string id, string collectionName = null) where Model : Base, new();

        ISingleResult<Model> Save<Model>(Model model, string collectionName = null) where Model : Base, new();
        ISingleResult<UpdateResult> Update<Model>(FilterDefinition<Model> filter, UpdateDefinition<Model> update, bool multi = false, string collectionName = null) where Model : Base, new();
        #endregion

    }
}
 