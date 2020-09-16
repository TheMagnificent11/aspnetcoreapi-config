using System;
using MediatR;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Post Command Interface
    /// </summary>
    /// <typeparam name="TId">ID type of entity being created</typeparam>
    /// <typeparam name="TRequestEntity">Request entity type</typeparam>
    public interface IPostCommand<TId, TRequestEntity> : IRequest<OperationResult<TId>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>, IConvertible
        where TRequestEntity : class
    {
    }
}
