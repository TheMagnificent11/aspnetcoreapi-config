using AspNetCoreApi.Infrastructure.Mediation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Serilog;

namespace Sample.Application.Teams.GetAll;

public sealed class GetAllQueryHandler : GetAllQueryHandler<Guid, Domain.Team, DatabaseContext, Team, GetAllQuery>
{
    public GetAllQueryHandler(IDbContextFactory<DatabaseContext> contextFactory, IMapper mapper, ILogger logger)
        : base(contextFactory, mapper, logger)
    {
    }
}
