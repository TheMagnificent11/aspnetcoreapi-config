using AspNetCoreApi.Infrastructure.Mediation;
using SampleApiWebApp.Domain;

namespace SampleApiWebApp.Controllers.Teams.Put
{
    public class PutTeamCommand : ITeam, IPutCommand<long>
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
