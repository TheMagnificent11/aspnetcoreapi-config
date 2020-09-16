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
    /// Put Command Handler
    /// </summary>
    /// <typeparam name="TId">Database entity ID type</typeparam>
    /// <typeparam name="TEntity">Database entity type</typeparam>
    /// <typeparam name="TRequest">Put request type</typeparam>
    /// <typeparam name="THandler">Type of handler inheriting <see cref="PutCommandHandler{TId, TEntity, TRequest, THandler}"/></typeparam>
    public abstract class PutCommandHandler<TId, TEntity, TRequest, THandler> :
        BaseRequestHandler<TId, TEntity, TRequest, OperationResult, THandler>,
        IRequestHandler<TRequest, OperationResult>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>, IConvertible
        where TEntity : class, IEntity<TId>
        where TRequest : class, IPutCommand<TId>
        where THandler : class, IRequestHandler<TRequest, OperationResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutCommandHandler{TId, TEntity, TRequest, THandler}"/> class
        /// </summary>
        /// <param name="databaseContext">Database context</param>
        /// <param name="logger">Logger</param>
        protected PutCommandHandler(IDatabaseContext databaseContext, ILogger logger)
            : base(databaseContext, logger)
        {
        }

        /// <summary>
        /// Handles the put request
        /// </summary>
        /// <param name="request">Put request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An <see cref="OperationResult"/> that reports success and any validation errors it was a bad request</returns>
        public async Task<OperationResult> Handle(TRequest request, CancellationToken cancellationToken)
        {
            if (request is null)
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

                try
                {
                    await this.BindToDomainEntityAndValidate(domainEntity, request, cancellationToken);

                    await this.DatabaseContext.SaveChangesAsync(cancellationToken);
                }
                catch (ValidationException ex)
                {
                    this.Logger.Information(ex, "Validation failed");
                    return OperationResult.Fail(ex.Errors);
                }

                return OperationResult.Success();
            }
        }

        /// <summary>
        /// Generate a domain entity from create entity request
        /// </summary>
        /// <param name="domainEntity">Domain entity read from the database to be updated</param>
        /// <param name="request">Create entity request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Asynchronous task</returns>
        /// <exception cref="ValidationException">Exception thrown when validation errors occur</exception>
        protected abstract Task BindToDomainEntityAndValidate(
            [NotNull] TEntity domainEntity,
            [NotNull] TRequest request,
            [NotNull] CancellationToken cancellationToken);
    }
}
