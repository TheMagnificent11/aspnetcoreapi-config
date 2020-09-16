using System;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using MediatR;

namespace SampleApiWebApp.Controllers.Teams.Post
{
    public sealed class PostTeamCommandHandler : IRequestHandler<PostTeamCommand, OperationResult>
    {
        /*
        public PostTeamHandler(IEntityRepository<Domain.Team, long> repository, ILogger logger)
            : base(repository, logger)
        {
        }

        protected override async Task<Domain.Team> GenerateAndValidateDomainEntity(
            [NotNull] PostTeamCommand request,
            [NotNull] ILogger logger,
            [NotNull] CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var teamName = request.Name.Trim();

            var query = new GetTeamsByName(teamName);
            var teamsWithSameName = await this.Repository
                .Query(query)
                .ToListAsync(cancellationToken);

            if (teamsWithSameName.Any())
            {
                var error = new ValidationFailure(nameof(request.Name), string.Format(Domain.Team.ErrorMessages.NameNotUniqueFormat, teamName));
#pragma warning disable CA1062 // Validate arguments of public methods
                logger.Information("Validation failed: a Team with the name {TeamName} already exists", request.Name);
#pragma warning restore CA1062 // Validate arguments of public methods
                throw new ValidationException(new ValidationFailure[] { error });
            }

            return Domain.Team.CreateTeam(teamName);
        }
        */

        public Task<OperationResult> Handle(PostTeamCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
