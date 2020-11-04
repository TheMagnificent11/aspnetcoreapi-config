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
    public abstract class BaseRequestHandler<TId, TEntity, TRequest, TResponse>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TEntity : class, IEntity<TId>
        where TRequest : class, IRequest<TResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRequestHandler{TId, TEntity, TRequest, TResponse}"/> class
        /// </summary>
        /// <param name="databaseContext">Database context</param>
        /// <param name="logger">Logger</param>
        protected BaseRequestHandler(IDatabaseContext databaseContext, ILogger logger)
        {
            if (logger is null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            this.DatabaseContext = databaseContext;
            this.Logger = logger.ForContext(this.GetType());
        }

        /// <summary>
        /// Gets the database context
        /// </summary>
        protected IDatabaseContext DatabaseContext { get; }

        /// <summary>
        /// Gets the logger
        /// </summary>
        protected ILogger Logger { get; }

        internal string GetLoggerTimedOperationName()
        {
            return $"{this.GetType().Name}.Handle";
        }

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
    }
}
