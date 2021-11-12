using AspNetCoreApi.Infrastructure.Mediation;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Serilog;

namespace Sample.Application.Teams.Delete;

public sealed class DeleteCommandHandler : DeleteCommandHandler<Guid, Domain.Team, DatabaseContext, DeleteCommand>
{
    public DeleteCommandHandler(IDbContextFactory<DatabaseContext> contextFactory, ILogger logger)
        : base(contextFactory, logger)
    {
    }

    protected override void DeleteDomainEntity(DatabaseContext context, Domain.Team domainEntity)
    {
        context.Set<Domain.Team>().Remove(domainEntity);
    }
}
