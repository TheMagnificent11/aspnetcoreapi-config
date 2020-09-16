using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using EntityManagement;
using MediatR;
using Serilog;

namespace SampleApiWebApp.Controllers.Players.Post
{
    public sealed class PostPlayerCommandHandler : IRequestHandler<PostPlayerCommand, OperationResult>
    {
        public Task<OperationResult> Handle(PostPlayerCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
