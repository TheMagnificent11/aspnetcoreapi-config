using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using EntityManagement;
using Serilog;

namespace SampleApiWebApp.Controllers.Players.Post
{
    public sealed class PostPlayerCommandHandler : PostCommandHandler<long, Domain.Player, PostPlayerCommand>
    {
        public PostPlayerCommandHandler(IDatabaseContext databaseContext, ILogger logger)
            : base(databaseContext, logger)
        {
        }

        protected override Task<Domain.Player> GenerateAndValidateDomainEntity(
            [NotNull] PostPlayerCommand request,
            [NotNull] CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
