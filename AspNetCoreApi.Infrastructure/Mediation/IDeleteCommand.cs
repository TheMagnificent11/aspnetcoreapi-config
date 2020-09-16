using System;
using MediatR;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Delete Command Interface
    /// </summary>
    /// <typeparam name="TId">Entity ID type</typeparam>
    public interface IDeleteCommand<TId> : IRequest<OperationResult>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>, IConvertible
    {
        /// <summary>
        /// Gets or sets the entity ID
        /// </summary>
        TId Id { get; set; }
    }
}
