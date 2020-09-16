using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;
using SampleApiWebApp.Domain;

namespace SampleApiWebApp.Controllers.Teams.Post
{
    public class PostTeamCommand : ITeam, IRequest<OperationResult>, IPostCommand<long>
    {
        public string Name { get; set; }
    }
}
