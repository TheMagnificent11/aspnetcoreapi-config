using AspNetCoreApi.Infrastructure.Mediation;
using Microsoft.EntityFrameworkCore;
using SampleApiWebApp.Data;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.Delete
{
    public sealed class DeleteCommandHandler : DeleteCommandHandler<long, Domain.Team, DatabaseContext, DeleteCommand>
    {
        public DeleteCommandHandler(IDbContextFactory<DatabaseContext> contextFactory, ILogger logger)
            : base(contextFactory, logger)
        {
        }

        protected override void DeleteDomainEntity(DatabaseContext context, Domain.Team domainEntity)
        {
            context.EntitySet<Domain.Team>().Remove(domainEntity);
        }
    }
}
