using AspNetCoreApi.Infrastructure.Mediation;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SampleApiWebApp.Data;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.GetAll
{
    public sealed class GetAllQueryHandler : GetAllQueryHandler<long, Domain.Team, DatabaseContext, Team, GetAllQuery>
    {
        public GetAllQueryHandler(IDbContextFactory<DatabaseContext> contextFactory, IMapper mapper, ILogger logger)
            : base(contextFactory, mapper, logger)
        {
        }
    }
}
