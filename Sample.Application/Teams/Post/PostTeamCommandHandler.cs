using AspNetCoreApi.Infrastructure.Mediation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Sample.Data;
using Sample.Data.Queries;
using Serilog;

namespace Sample.Application.Teams.Post;

public sealed class PostTeamCommandHandler : PostCommandHandler<Guid, Domain.Team, DatabaseContext, PostTeamCommand>
{
    public PostTeamCommandHandler(IDbContextFactory<DatabaseContext> contextFactory, ILogger logger)
        : base(contextFactory, logger)
    {
    }

    protected override async Task<Domain.Team> GenerateAndValidateDomainEntity(
        DatabaseContext context,
        PostTeamCommand request,
        CancellationToken cancellationToken)
    {
        if (context is null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var teamName = request.Name.Trim();

        var teamsWithSameName = await context
            .Set<Domain.Team>()
            .GetTeamsByName(request.Name)
            .ToArrayAsync(cancellationToken);

        if (teamsWithSameName.Any())
        {
            var error = new ValidationFailure(nameof(request.Name), string.Format(Domain.Team.ErrorMessages.NameNotUniqueFormat, teamName));
            this.Logger.Information("Validation failed: a Team with the name {TeamName} already exists", request.Name);
            throw new ValidationException(new ValidationFailure[] { error });
        }

        return Domain.Team.CreateTeam(teamName);
    }
}
