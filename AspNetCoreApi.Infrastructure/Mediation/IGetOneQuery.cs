using System;
using MediatR;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Get One Entity Query Interface
    /// </summary>
    /// <typeparam name="TId">Entity ID type</typeparam>
    /// <typeparam name="TResponseEntity">Reponse entity type</typeparam>
    public interface IGetOneQuery<TId, TResponseEntity> : IRequest<OperationResult<TResponseEntity>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TResponseEntity : class
    {
        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        TId Id { get; set; }
    }
}
