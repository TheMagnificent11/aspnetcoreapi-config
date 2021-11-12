using AspNetCoreApi.Infrastructure.Mediation;
using Sample.Domain;

namespace Sample.Application.Teams.Put;

public class PutTeamCommand : ITeam, IPutCommand<Guid>
{
    public PutTeamCommand(Guid id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public Guid Id { get; }

    public string Name { get; }
}
