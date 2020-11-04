using AspNetCoreApi.Infrastructure.Mediation;
using AutoMapper;
using EntityManagement;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.GetAll
{
    public sealed class GetAllQueryHandler : GetAllQueryHandler<long, Domain.Team, Team, GetAllQuery>
    {
        public GetAllQueryHandler(IDatabaseContext databaseContext, IMapper mapper, ILogger logger)
            : base(databaseContext, mapper, logger)
        {
        }
    }
}
