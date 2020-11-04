using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;

namespace SampleApiWebApp.Controllers.Teams.GetOne
{
    public class GetTeamQuery : IGetOneQuery<long, Team>
    {
        public long Id { get; set; }
    }
}
