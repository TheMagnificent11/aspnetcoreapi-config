using AspNetCoreApi.Infrastructure.Mediation;
using Sample.Domain;

namespace Sample.Application.Teams.Post;

public class PostTeamCommand : ITeam, IPostCommand<Guid>
{
    public PostTeamCommand(string name)
    {
        this.Name = name;
    }

    public string Name { get; }
}
