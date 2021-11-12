using AutoMapper;

namespace Sample.Application.Teams;

public sealed class TeamMappings : Profile
{
    public TeamMappings()
    {
        this.CreateMap<Domain.Team, Team>();
    }
}
