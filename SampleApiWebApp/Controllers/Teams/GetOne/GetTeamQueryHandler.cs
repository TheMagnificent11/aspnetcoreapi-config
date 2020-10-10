using AspNetCoreApi.Infrastructure.Mediation;
using AutoMapper;
using EntityManagement;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.GetOne
{
    public sealed class GetTeamQueryHandler : GetOneQueryHandler<long, Domain.Team, Team, GetTeamQuery>
    {
        public GetTeamQueryHandler(IDatabaseContext databaseContext, IMapper mapper, ILogger logger)
            : base(databaseContext, mapper, logger)
        {
        }
    }
}
