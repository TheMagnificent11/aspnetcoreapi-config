using AspNetCoreApi.Infrastructure.Mediation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Serilog;

namespace Sample.Application.Teams.GetOne;

public sealed class GetTeamQueryHandler : GetOneQueryHandler<Guid, Domain.Team, DatabaseContext, Team, GetTeamQuery>
{
    public GetTeamQueryHandler(IDbContextFactory<DatabaseContext> contextFactory, IMapper mapper, ILogger logger)
        : base(contextFactory, mapper, logger)
    {
    }
}
