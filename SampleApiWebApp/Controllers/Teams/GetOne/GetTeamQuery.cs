using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;

namespace SampleApiWebApp.Controllers.Teams.GetOne
{
    public class GetTeamQuery : IRequest<OperationResult<Team>>
    {
        public long Id { get; set; }
    }
}
