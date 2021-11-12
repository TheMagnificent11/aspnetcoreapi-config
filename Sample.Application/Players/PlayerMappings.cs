using AutoMapper;

namespace Sample.Application.Players;

public sealed class PlayerMappings : Profile
{
    public PlayerMappings()
    {
        this.CreateMap<Domain.Player, Player>();
    }
}
