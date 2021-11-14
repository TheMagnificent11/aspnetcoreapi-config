using AspNetCoreApi.Infrastructure.Mediation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Serilog;

namespace Sample.Application.Teams.GetAll;

public sealed class GetAllTeamsQueryHandler : GetAllQueryHandler<Guid, Domain.Team, DatabaseContext, Team, GetAllTeamsQuery>
{
    public GetAllTeamsQueryHandler(IDbContextFactory<DatabaseContext> contextFactory, IMapper mapper, ILogger logger)
        : base(contextFactory, mapper, logger)
    {
    }
}
