using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EntityManagement;
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
    /// <typeparam name="TResponseEntity">Entity response type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="THandler">Type of handler inheriting <see cref="GetAllQueryHandler{TId, TEntity, TResponseEntity, TRequest, THandler}"/></typeparam>
    public abstract class GetAllQueryHandler<TId, TEntity, TResponseEntity, TRequest, THandler> :
        BaseRequestHandler<TId, TEntity, TRequest, OperationResult<IEnumerable<TResponseEntity>>, THandler>,
        IRequestHandler<TRequest, OperationResult<IEnumerable<TResponseEntity>>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>, IConvertible
        where TEntity : class, IEntity<TId>
        where TResponseEntity : class
        where TRequest : class, IGetAllQuery<TResponseEntity>
        where THandler : class, IRequestHandler<TRequest, OperationResult<IEnumerable<TResponseEntity>>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllQueryHandler{TId, TEntity, TResponseEntity, TRequest, THandler}"/> class
        /// </summary>
        /// <param name="databaseContext">Database context</param>
        /// <param name="mapper"></param>
        /// <param name="logger">Logger</param>
        protected GetAllQueryHandler(IDatabaseContext databaseContext, IMapper mapper, ILogger logger)
            : base(databaseContext, logger)
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
            {
                var domainEntities = await this.DatabaseContext
                    .EntitySet<TEntity>()
                    .ToArrayAsync(cancellationToken);

                var responseEntities = this.Mapper.Map<IEnumerable<TResponseEntity>>(domainEntities);

                return OperationResult.Success(responseEntities);
            }
        }
    }
}
