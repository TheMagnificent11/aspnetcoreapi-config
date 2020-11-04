using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EntityManagement;
using EntityManagement.Core;
using MediatR;
using Serilog;
using Serilog.Context;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Delete Command Handler
    /// </summary>
    /// <typeparam name="TId">Database entity ID type</typeparam>
    /// <typeparam name="TEntity">Database entity type</typeparam>
    /// <typeparam name="TRequest">Delete command type</typeparam>
    public abstract class DeleteCommandHandler<TId, TEntity, TRequest> :
        BaseRequestHandler<TId, TEntity, TRequest, OperationResult>,
        IRequestHandler<TRequest, OperationResult>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TEntity : class, IEntity<TId>
        where TRequest : class, IDeleteCommand<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteCommandHandler{TId, TEntity, TRequest}"/> class
        /// </summary>
        /// <param name="databaseContext">Database context</param>
        /// <param name="logger">Logger</param>
        protected DeleteCommandHandler(IDatabaseContext databaseContext, ILogger logger)
            : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Handles the delete request
        /// </summary>
        /// <param name="request">Delete request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An <see cref="OperationResult"/> that reports success and any validation errors it was a bad request</returns>
        public async Task<OperationResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (LogContext.PushProperty(LoggingProperties.EntityType, typeof(TEntity).Name))
            using (LogContext.PushProperty(LoggingProperties.EntityId, request.Id))
            using (this.Logger.BeginTimedOperation(this.GetLoggerTimedOperationName()))
            {
                var domainEntity = await this.GetById(request.Id, cancellationToken);

                if (domainEntity == null)
                {
                    return OperationResult.NotFound();
                }

                var validationErrors = await this.ValidateDeletion(domainEntity, request, this.Logger, cancellationToken);

                if (validationErrors != null && validationErrors.Any())
                {
                    return OperationResult.Fail(validationErrors);
                }

                this.DeleteDomainEntity(domainEntity);

                await this.DatabaseContext.SaveChangesAsync(cancellationToken);

                return OperationResult.Success();
            }
        }

        /// <summary>
        /// Validates whether deletion is allowed
        /// </summary>
        /// <param name="domainEntity">Entity to delete</param>
        /// <param name="request">Delete request</param>
        /// <param name="logger">The logger</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Dictionary of validation errors keyed by request field name</returns>
        protected virtual Task<IDictionary<string, IEnumerable<string>>> ValidateDeletion(
            TEntity domainEntity,
            TRequest request,
            ILogger logger,
            CancellationToken cancellationToken)
        {
            IDictionary<string, IEnumerable<string>> validationErrors = new Dictionary<string, IEnumerable<string>>();
            return Task.FromResult(validationErrors);
        }

        /// <summary>
        /// Executes the delete operation
        /// </summary>
        /// <param name="domainEntity">Domain eneity to delete</param>
        /// <remarks>
        /// If <paramref name="domainEntity"/> implemnets <see cref="ISoftDeleteEntity"/>, then here would be the place to call the domain method to soft-delete the entity
        /// </remarks>
        protected abstract void DeleteDomainEntity(TEntity domainEntity);
    }
}
