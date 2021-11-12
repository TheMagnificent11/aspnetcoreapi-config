using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sample.Api.Constants;
using Sample.Api.Controllers.V1.Requests;
using Sample.Application.Teams;
using Sample.Application.Teams.Delete;
using Sample.Application.Teams.GetAll;
using Sample.Application.Teams.GetOne;
using Sample.Application.Teams.Post;
using Sample.Application.Teams.Put;

namespace Sample.Api.Controllers.Teams.V1;

[ApiController]
[Route("v1/[controller]")]
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
    public async Task<IActionResult> GetOne([FromRoute] Guid id, CancellationToken cancellationToken = default)
    {
        var request = new GetTeamQuery(id);
        var result = await this.mediator.Send(request, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPost]
    [Consumes(ContentTypes.ApplicationJson)]
    [Produces(ContentTypes.ApplicationJson)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
    public async Task<IActionResult> Post(
        [FromBody] TeamRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var command = new PostTeamCommand(request.Name);
        var result = await this.mediator.Send(command, cancellationToken);

        return result.ToActionResult();
    }

    [HttpPut("{id}")]
    [Consumes(ContentTypes.ApplicationJson)]
    [Produces(ContentTypes.ApplicationJson)]
    [ProducesResponseType(200)]
    [ProducesResponseType(400, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Put(
        [FromRoute] Guid id,
        [FromBody] TeamRequest request,
        CancellationToken cancellationToken = default)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var command = new PutTeamCommand(id, request.Name);
        var result = await this.mediator.Send(command, cancellationToken);

        return result.ToActionResult();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var request = new DeleteCommand(id);
        var result = await this.mediator.Send(request, cancellationToken);

        return result.ToActionResult();
    }
}
