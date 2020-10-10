using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;
using SampleApiWebApp.Domain;

namespace SampleApiWebApp.Controllers.Teams.Put
{
    public class PutTeamCommand : ITeam, IRequest<OperationResult>, IPutCommand<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
