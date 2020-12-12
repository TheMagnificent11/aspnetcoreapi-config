using System;
using System.Threading;
using System.Threading.Tasks;
using EntityManagement.Core;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Context;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Put Command Handler
    /// </summary>
    /// <typeparam name="TId">Database entity ID type</typeparam>
    /// <typeparam name="TEntity">Database entity type</typeparam>
    /// <typeparam name="TContext">Datbase context type</typeparam>
    /// <typeparam name="TRequest">Put request type</typeparam>
    public abstract class PutCommandHandler<TId, TEntity, TContext, TRequest> :
        BaseRequestHandler<TId, TEntity, TContext, TRequest, OperationResult>,
        IRequestHandler<TRequest, OperationResult>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TEntity : class, IEntity<TId>
        where TContext : DbContext
        where TRequest : class, IPutCommand<TId>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PutCommandHandler{TId, TEntity, TContext, TRequest}"/> class
        /// </summary>
        /// <param name="contextFactory">Database context factory</param>
        /// <param name="logger">Logger</param>
        protected PutCommandHandler(IDbContextFactory<TContext> contextFactory, ILogger logger)
            : base(contextFactory, logger)
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
            using (var context = this.DatabaseContextFactory.CreateDbContext())
            {
                var domainEntity = await GetById(context, request.Id, cancellationToken);

                if (domainEntity == null)
                {
                    return OperationResult.NotFound();
                }

                try
                {
                    await this.BindToDomainEntityAndValidate(context, domainEntity, request, cancellationToken);

                    await context.SaveChangesAsync(cancellationToken);
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
        /// <param name="context">Database context</param>
        /// <param name="domainEntity">Domain entity read from the database to be updated</param>
        /// <param name="request">Create entity request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Asynchronous task</returns>
        /// <exception cref="ValidationException">Exception thrown when validation errors occur</exception>
        protected abstract Task BindToDomainEntityAndValidate(
            TContext context,
            TEntity domainEntity,
            TRequest request,
            CancellationToken cancellationToken);
    }
}
