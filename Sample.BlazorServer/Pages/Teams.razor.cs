using MediatR;
using Microsoft.AspNetCore.Components;
using Sample.Application.Teams;
using Sample.Application.Teams.GetAll;

namespace Sample.BlazorServer.Pages;

public partial class Teams
{
    [Inject]
    private IMediator Mediator { get; set; }

    private IDictionary<string, IEnumerable<string>> Errors { get; set; }

    private IEnumerable<string> PageErrors =>
        this.Errors == null
            ? new List<string>()
            : this.Errors.Where(x => string.IsNullOrEmpty(x.Key))
                .SelectMany(x => x.Value);

    private IEnumerable<Team> Data { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var query = new GetAllTeamsQuery();
        var result = await this.Mediator.Send(query);

        if (!result.IsSuccess)
        {
            this.Errors = result.Errors;
            return;
        }

        this.Data = result.Data;
    }
}
