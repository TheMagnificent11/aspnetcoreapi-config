using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using AutoMapper;
using EntityManagement;
using MediatR;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.GetOne
{
    public sealed class GetTeamQueryHandler : IRequestHandler<GetTeamQuery, OperationResult<Team>>
    {
        public Task<OperationResult<Team>> Handle(GetTeamQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
