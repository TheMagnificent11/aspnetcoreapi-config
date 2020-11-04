using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SampleApiWebApp.Constants;
using SampleApiWebApp.Controllers.Teams.Delete;
using SampleApiWebApp.Controllers.Teams.GetAll;
using SampleApiWebApp.Controllers.Teams.GetOne;
using SampleApiWebApp.Controllers.Teams.Post;
using SampleApiWebApp.Controllers.Teams.Put;

namespace SampleApiWebApp.Controllers.Teams
{
    [ApiController]
    [Route("[controller]")]
    public sealed class TeamsController : Controller
    {
        private readonly IMediator mediator;

        public TeamsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Team>))]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
        {
            var request = new GetAllQuery();
            var result = await this.mediator.Send(request, cancellationToken);

            return result.ToActionResult();
        }

        [HttpGet("{id}")]
        [Consumes(ContentTypes.ApplicationJson)]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(200, Type = typeof(Team))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetOne([FromRoute] long id, CancellationToken cancellationToken = default)
        {
            var request = new GetTeamQuery { Id = id };
            var result = await this.mediator.Send(request, cancellationToken);

            return result.ToActionResult();
        }

        [HttpPost]
        [Consumes(ContentTypes.ApplicationJson)]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        public async Task<IActionResult> Post(
            [FromBody] PostTeamCommand request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var result = await this.mediator.Send(request, cancellationToken);

            return result.ToActionResult();
        }

        [HttpPut("{id}")]
        [Consumes(ContentTypes.ApplicationJson)]
        [Produces(ContentTypes.ApplicationJson)]
        [ProducesResponseType(200)]
        [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(
            [FromRoute] long id,
            [FromBody] PutTeamCommand request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            request.Id = id;

            var result = await this.mediator.Send(request, cancellationToken);

            return result.ToActionResult();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(
            [FromRoute] long id,
            CancellationToken cancellationToken = default)
        {
            var request = new DeleteCommand { Id = id };
            var result = await this.mediator.Send(request, cancellationToken);

            return result.ToActionResult();
        }
    }
}
