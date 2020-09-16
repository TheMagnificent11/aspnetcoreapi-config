using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EntityManagement;
using EntityManagement.Core;
using MediatR;
using Serilog;
using Serilog.Context;

namespace AspNetCoreApi.Infrastructure.Mediation
{
    /// <summary>
    /// Get One Query Handler
    /// </summary>
    /// <typeparam name="TId">Database entity ID type</typeparam>
    /// <typeparam name="TEntity">Database entity type</typeparam>
    /// <typeparam name="TResponseEntity">Entity response type</typeparam>
    /// <typeparam name="TRequest">Request type</typeparam>
    /// <typeparam name="THandler">Type of handler inheriting <see cref="GetOneQueryHandler{TId, TEntity, TResponseEntity, TRequest, THandler}"/></typeparam>
    public abstract class GetOneQueryHandler<TId, TEntity, TResponseEntity, TRequest, THandler> :
        BaseRequestHandler<TId, TEntity, TRequest, OperationResult<TResponseEntity>, THandler>,
        IRequestHandler<TRequest, OperationResult<TResponseEntity>>
        where TId : IComparable, IComparable<TId>, IEquatable<TId>, IConvertible
        where TEntity : class, IEntity<TId>
        where TResponseEntity : class
        where TRequest : class, IGetOneQuery<TId, TResponseEntity>
        where THandler : class, IRequestHandler<TRequest, OperationResult<TResponseEntity>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetOneQueryHandler{TId, TEntity, TResponseEntity, TRequest, THandler}"/> class
        /// </summary>
        /// <param name="databaseContext">Database context</param>
        /// <param name="mapper">Mapper</param>
        /// <param name="logger">Logger</param>
        protected GetOneQueryHandler(IDatabaseContext databaseContext, IMapper mapper, ILogger logger)
            : base(databaseContext, logger)
        {
            this.Mapper = mapper;
        }

        /// <summary>
        /// Gets the mapper
        /// </summary>
        protected IMapper Mapper { get; }

        /// <summary>
        /// Handles get one entity requests
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>
        /// Operation result containing the response entity as the data if successful,
        /// otherwise not found operation result
        /// </returns>
        public async Task<OperationResult<TResponseEntity>> Handle(
            TRequest request,
            CancellationToken cancellationToken)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            using (LogContext.PushProperty(LoggingProperties.EntityType, typeof(TEntity).Name))
            using (LogContext.PushProperty(LoggingProperties.EntityId, request.Id))
            using (this.Logger.BeginTimedOperation(this.GetLoggerTimedOperationName()))
            {
                var entity = await this.GetById(request.Id, cancellationToken);
                if (entity == null)
                {
                    return OperationResult.NotFound<TResponseEntity>();
                }

                var result = this.Mapper.Map<TResponseEntity>(entity);

                return OperationResult.Success(result);
            }
        }
    }
}
