using AspNetCoreApi.Infrastructure.Mediation;

namespace Sample.Application.Teams.GetOne;

public class GetTeamQuery : IGetOneQuery<Guid, Team>
{
    public GetTeamQuery(Guid id)
    {
        this.Id = id;
    }

    public Guid Id { get; }
}
