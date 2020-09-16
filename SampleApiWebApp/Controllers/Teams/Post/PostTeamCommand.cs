using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;
using SampleApiWebApp.Domain;

namespace SampleApiWebApp.Controllers.Teams.Post
{
    public class PostTeamCommand : ITeam, IRequest<OperationResult>
    {
        public string Name { get; set; }
    }
}
