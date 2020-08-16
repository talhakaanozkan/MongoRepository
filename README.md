# Mongo Repository
![](https://i.imgur.com/9NEK8yv.png)
This easy to use library implements the repository pattern on top of Official MongoDB C# driver.
This project works .NET Core 3.1 and greater

## DB Connector

You can connect DB with `MongoRepositoryPattern` also more than one methods 

### Local Machine

```csharp
MongoRepositoryPattern Connector = new MongoRepositoryPattern(new MongoDataContext());
```

### Remote DB:

```csharp
MongoRepositoryPattern Connector = new MongoRepositoryPattern(new MongoDataContext("root", "*******", "admin.mlab.com", "27017", "admin", false));
```

### with String:

```csharp
MongoRepositoryPattern Connector = new MongoRepositoryPattern(new MongoDataContext("root:*******.@admin.mlab.com:27017", "admin"));
```


## Methods

<details>
  <summary>Collection</summary>
  
```csharp
string CollectionName<Model>() where Model : Base, new();
```
```csharp
IMongoCollection<Model> Collection<Model>(string collectionName = null) where Model : Base, new();
```
</details>

<details>
  <summary>With Async</summary>
  
```csharp
Task<ISingleResult<Model>> InsertOneAsync<Model>(Model model, string collectionName = null) where Model : Base, new();
```
```csharp
Task<IListResult<Model>> InsertManyAsync<Model>(List<Model> model, string collectionName = null) where Model : Base, new();
```

```csharp
Task<ISingleResult<long>> CountDocumentsAsync<Model>(string collectionName = null) where Model : Base, new();
Task<ISingleResult<long>> CountDocumentsAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
```

```csharp
Task<ISingleResult<DeleteResult>> DeleteOneAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
```

```csharp
Task<ISingleResult<DeleteResult>> DeleteManyAsync<Model>(string collectionName = null) where Model : Base, new();
Task<ISingleResult<DeleteResult>> DeleteManyAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
```

```csharp
Task<ISingleResult<bool>> ExistAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName) where Model : Base, new();
```

```csharp
Task<ISingleResult<Model>> GetAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName) where Model : Base, new();
```

```csharp
Task<IListResult<Model>> GetListAsync<Model>(string collectionName = null) where Model : Base, new();
Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, int limit, string collectionName = null) where Model : Base, new();
Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, string collectionName = null) where Model : Base, ne();
Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, string collectionName = null) where Model: Base, new();
Task<IListResult<Model>> GetListAsync<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, int skip, string collectionName = null)where Model : Base, new();
```
```csharp
Task<ISingleResult<string>> IndexesCreateOneAsync<Model>(string text, string collectionName = null) where Model : Base, new();
```
```csharp
Task<ISingleResult<Model>> GetSingleAsync<Model>(string id, string collectionName = null) where Model : Base, new();
```
```csharp
Task<ISingleResult<Model>> SaveAsync<Model>(Model model, string collectionName = null) where Model : Base, new();
```
```csharp
Task<ISingleResult<UpdateResult>> UpdateAsync<Model>(FilterDefinition<Model> filter, UpdateDefinition<Model> update, bool multi = false, string collectionName = null) where Model : Base, new();
```
</details>

<details>
  <summary>Without Async</summary>
  
```csharp
ISingleResult<Model> InsertOne<Model>(Model model, string collectionName = null) where Model : Base, new();
```
```csharp
IListResult<Model> InsertMany<Model>(List<Model> model, string collectionName = null) where Model : Base, new();
```

```csharp
ISingleResult<long> CountDocuments<Model>(string collectionName = null) where Model : Base, new();
ISingleResult<long> CountDocuments<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
```

```csharp
ISingleResult<DeleteResult> DeleteOne<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
```

```csharp
ISingleResult<DeleteResult> DeleteMany<Model>(string collectionName = null) where Model : Base, new();
ISingleResult<DeleteResult> DeleteMany<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
```

```csharp
ISingleResult<bool> Exist<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
```

```csharp
ISingleResult<Model> Get<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
```

```csharp
IListResult<Model> GetList<Model>(string collectionName = null) where Model : Base, new();
IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string collectionName = null) where Model : Base, new();
IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, int limit, string collectionName = null) where Model : Base, new();
IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, string collectionName = null) where Model : Base, new();
IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, string collectionName = null) where Model : Base, ne();
IListResult<Model> GetList<Model>(Expression<Func<Model, bool>> predicate, string sortField, bool descending, int limit, int skip, string collectionName = null) where Model :Base, new();
```
```csharp
ISingleResult<string> IndexesCreateOne<Model>(string text, string collectionName = null) where Model : Base, new();
```
```csharp
ISingleResult<Model> GetSingle<Model>(string id, string collectionName = null) where Model : Base, new();
```
```csharp
ISingleResult<Model> Save<Model>(Model model, string collectionName = null) where Model : Base, new();
```
```csharp
ISingleResult<UpdateResult> Update<Model>(FilterDefinition<Model> filter, UpdateDefinition<Model> update, bool multi = false, string collectionName = null) where Model : Base, new();
```
</details>

## Sample for ASP.NET MVC Core

```csharp
[HttpPost]
public IActionResult Save()
{
    MongoRepositoryPattern Connector = new MongoRepositoryPattern(new MongoDataContext());

    Product Model = new Product()
    {
        Status = Status.Active,
        Name = "iPhone 11 - 64 GB"
    };

    ISingleResult<Product> SaveResult = Connector.Save<Product>(Model);

    return Json(SaveResult.DidError ?
        new { Status = false, Msg = SaveResult.Message, Model = default(Product) } :
        new { Status = true, Msg = "Success", Model = SaveResult.Model });
    }
}
```
