using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;

namespace SampleApiWebApp.Controllers.Players.GetOne
{
    public class GetPlayerRequest : IGetOneQuery<long, Player>
    {
        public long Id { get; set; }
    }
}
