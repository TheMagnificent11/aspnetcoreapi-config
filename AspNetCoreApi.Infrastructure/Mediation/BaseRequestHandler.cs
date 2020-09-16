using System;
using System.Threading;
using System.Threading.Tasks;
using EntityManagement;
using EntityManagement.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Base Request Handler
    /// </summary>
    /// <typeparam name="TId">Database entity ID type</typeparam>
    /// <typeparam name="TEntity">Database entity type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="TResponse">Response type</typeparam>
    /// <typeparam name="THandler">Type of handler inheriting <see cref="BaseRequestHandler{TId, TEntity, TRequest, TResponse, THandler}"/></typeparam>
    public abstract class BaseRequestHandler<TId, TEntity, TRequest, TResponse, THandler>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>, IConvertible
        where TEntity : class, IEntity<TId>
        where TRequest : class, IRequest<TResponse>
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRequestHandler{TId, TEntity, TRequest, TResponse, THandler}"/> class
        /// </summary>
        /// <param name="databaseContext">Database context</param>
        /// <param name="logger">Logger</param>
        protected BaseRequestHandler(IDatabaseContext databaseContext, ILogger logger)
        {
            this.DatabaseContext = databaseContext;
            this.Logger = logger.ForContext<THandler>();
        }

        /// <summary>
        /// Gets the database context
        /// </summary>
        protected IDatabaseContext DatabaseContext { get; }

        /// <summary>
        /// Gets the logger
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets a domain entity using an ID-lookup
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Task with domain entity if it exists, task with null</returns>
        protected Task<TEntity> GetById(TId id, CancellationToken cancellationToken)
        {
            return this.DatabaseContext
                .EntitySet<TEntity>()
                .SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);
        }

        internal string GetLoggerTimedOperationName()
        {
            return $"{typeof(THandler).Name}.Handle";
        }
    }
}
