using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using EntityManagement;
using EntityManagement.Core;
using FluentValidation;
using MediatR;
using Serilog;
using Serilog.Context;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Post Command Handler
    /// </summary>
    /// <typeparam name="TId">Database entity ID type</typeparam>
    /// <typeparam name="TEntity">Database entity type</typeparam>
    /// <typeparam name="TRequest">Post request type</typeparam>
    public abstract class PostCommandHandler<TId, TEntity, TRequest> :
        BaseRequestHandler<TId, TEntity, TRequest, OperationResult<TId>>,
        IRequestHandler<TRequest, OperationResult<TId>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>, IConvertible
        where TEntity : class, IEntity<TId>
        where TRequest : class, IPostCommand<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PostCommandHandler{TId, TEntity, TRequest}"/> class
        /// </summary>
        /// <param name="databaseContext">Database context</param>
        /// <param name="logger">Logger</param>
        protected PostCommandHandler(IDatabaseContext databaseContext, ILogger logger)
            : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Handles post requests
        /// </summary>
        /// <param name="request">Post request</param>
        /// <param name="cancellationToken">Canellation token</param>
        /// <returns>
        /// An <see cref="OperationResult{T}"/> containing the ID of the created entity if successful,
        /// otherwise bad request operation result containing errors collection
        /// </returns>
        public async Task<OperationResult<TId>> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (LogContext.PushProperty(LoggingProperties.EntityType, typeof(TEntity).Name))
            using (this.Logger.BeginTimedOperation(this.GetLoggerTimedOperationName()))
            {
                try
                {
                    var entity = await this.GenerateAndValidateDomainEntity(request, cancellationToken);

                    this.DatabaseContext
                        .EntitySet<TEntity>()
                        .Add(entity);

                    await this.DatabaseContext.SaveChangesAsync(cancellationToken);

                    return OperationResult.Success(entity.Id);
                }
                catch (ValidationException ex)
                {
                    this.Logger.Information(ex, "Validation failed");
                    return OperationResult.Fail<TId>(ex.Errors);
                }
            }
        }

        /// <summary>
        /// Generate a domain entity from create entity request
        /// </summary>
        /// <param name="request">Create entity request</param>
        /// <param name="cancellationToken">Canellation token</param>
        /// <returns>Entity to be created</returns>
        /// <exception cref="ValidationException">Exception thrown when validation errors occur</exception>
        protected abstract Task<TEntity> GenerateAndValidateDomainEntity(
            [NotNull] TRequest request,
            [NotNull] CancellationToken cancellationToken);
    }
}
