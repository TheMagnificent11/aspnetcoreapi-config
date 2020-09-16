using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;
using SampleApiWebApp.Domain;

namespace SampleApiWebApp.Controllers.Players.Post
{
    public class PostPlayerCommand : IPlayer, IRequest<OperationResult<Player>>, IPostCommand<long>
    {
        public string GivenName { get; set; }

        public string Surname { get; set; }

        public long TeamId { get; set; }

        public int Number { get; set; }
    }
}
