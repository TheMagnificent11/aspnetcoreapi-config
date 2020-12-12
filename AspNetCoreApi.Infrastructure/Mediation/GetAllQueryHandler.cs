using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EntityManagement.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Context;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Get All Query Handler
    /// </summary>
    /// <typeparam name="TId">Database entity ID type</typeparam>
    /// <typeparam name="TEntity">Database entity type</typeparam>
    /// <typeparam name="TContext">Datbase context type</typeparam>
    /// <typeparam name="TResponseEntity">Entity response type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    public abstract class GetAllQueryHandler<TId, TEntity, TContext, TResponseEntity, TRequest> :
        BaseRequestHandler<TId, TEntity, TContext, TRequest, OperationResult<IEnumerable<TResponseEntity>>>,
        IRequestHandler<TRequest, OperationResult<IEnumerable<TResponseEntity>>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>
        where TEntity : class, IEntity<TId>
        where TContext : DbContext
        where TResponseEntity : class
        where TRequest : class, IGetAllQuery<TResponseEntity>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllQueryHandler{TId, TEntity, TContext, TResponseEntity, TRequest}"/> class
        /// </summary>
        /// <param name="contextFactory">Database context factory</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        protected GetAllQueryHandler(IDbContextFactory<TContext> contextFactory, IMapper mapper, ILogger logger)
            : base(contextFactory, logger)
        {
            this.Mapper = mapper;
        }

        /// <summary>
        /// Gets the mapper
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// Handlers get all request
        /// </summary>
        /// <param name="request">Get all request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>An enumerable list of entities</returns>
        public async Task<OperationResult<IEnumerable<TResponseEntity>>> Handle(
            TRequest request,
            CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (LogContext.PushProperty(LoggingProperties.EntityType, typeof(TEntity).Name))
            using (this.Logger.BeginTimedOperation(this.GetLoggerTimedOperationName()))
            using (var context = this.DatabaseContextFactory.CreateDbContext())
            {
                var domainEntities = await context
                    .Set<TEntity>()
                    .ToArrayAsync(cancellationToken);

                var responseEntities = this.Mapper.Map<IEnumerable<TResponseEntity>>(domainEntities);

                return OperationResult.Success(responseEntities);
            }
        }
    }
}
