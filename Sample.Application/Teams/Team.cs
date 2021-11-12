using Sample.Domain;

namespace Sample.Application.Teams;

public class Team : ITeam
{
    public Guid Id { get; set; }

    public string Name { get; set; }
}
