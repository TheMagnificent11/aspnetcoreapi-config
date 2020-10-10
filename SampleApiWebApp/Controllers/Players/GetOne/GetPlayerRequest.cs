using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;

namespace SampleApiWebApp.Controllers.Players.GetOne
{
    public class GetPlayerRequest : IRequest<OperationResult<Player>>
    {
        public long Id { get; set; }
    }
}
