using AspNetCoreApi.Infrastructure.Mediation;
using SampleApiWebApp.Domain;

namespace SampleApiWebApp.Controllers.Teams.Post
{
    public class PostTeamCommand : ITeam, IPostCommand<long>
    {
        public string Name { get; set; }
    }
}
