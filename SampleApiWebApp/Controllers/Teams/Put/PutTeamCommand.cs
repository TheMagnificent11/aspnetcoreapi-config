using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;
using SampleApiWebApp.Domain;

namespace SampleApiWebApp.Controllers.Teams.Put
{
    public sealed class PutTeamCommand : ITeam, IRequest<OperationResult>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
