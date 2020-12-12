using AspNetCoreApi.Infrastructure.Mediation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SampleApiWebApp.Data;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.GetOne
{
    public sealed class GetTeamQueryHandler : GetOneQueryHandler<long, Domain.Team, DatabaseContext, Team, GetTeamQuery>
    {
        public GetTeamQueryHandler(IDbContextFactory<DatabaseContext> contextFactory, IMapper mapper, ILogger logger)
            : base(contextFactory, mapper, logger)
        {
        }
    }
}
