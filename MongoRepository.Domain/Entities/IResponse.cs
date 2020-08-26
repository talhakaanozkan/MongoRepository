using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MongoRepository.Domain.Entities
{
    public interface IResult
    {
        string Message { get; set; }

        bool HasError { get; set; }
    }

    public interface ISingleResult<TModel> : IResult
    {
        TModel Model { get; set; }
    }

    public interface IListResult<TModel> : IResult
    {
        IEnumerable<TModel> Model { get; set; }
    }

    public class Result : IResult
    {
        public string Message { get; set; } = string.Empty;

        public bool HasError { get; set; } = false;
    }

    public class SingleResult<TModel> : ISingleResult<TModel>
    {
        public string Message { get; set; } = string.Empty;

        public bool HasError { get; set; } = false;

        public TModel Model { get; set; } = default;
    }

    public class ListResult<TModel> : IListResult<TModel>
    {
        public string Message { get; set; } = string.Empty;

        public bool HasError { get; set; } = false;

        public IEnumerable<TModel> Model { get; set; } = Enumerable.Empty<TModel>();
    }
}
