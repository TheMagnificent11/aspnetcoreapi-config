using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using Microsoft.EntityFrameworkCore;
using SampleApiWebApp.Data;
using Serilog;

namespace SampleApiWebApp.Controllers.Players.Post
{
    public sealed class PostPlayerCommandHandler : PostCommandHandler<long, Domain.Player, DatabaseContext, PostPlayerCommand>
    {
        public PostPlayerCommandHandler(IDbContextFactory<DatabaseContext> contextFactory, ILogger logger)
            : base(contextFactory, logger)
        {
        }

        protected override Task<Domain.Player> GenerateAndValidateDomainEntity(
            DatabaseContext context,
            PostPlayerCommand request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
