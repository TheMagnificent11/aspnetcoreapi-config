using System;
using MediatR;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Put Command Interface
    /// </summary>
    /// <typeparam name="TId">Entity ID type</typeparam>
    public interface IPutCommand<TId> : IRequest<OperationResult>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
    {
        /// <summary>
        /// Gets or sets the entity ID
        /// </summary>
        TId Id { get; set; }
    }
}
