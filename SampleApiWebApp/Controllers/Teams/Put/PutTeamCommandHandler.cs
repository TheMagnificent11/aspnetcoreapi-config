using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using SampleApiWebApp.Data;
using SampleApiWebApp.Data.Queries;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.Put
{
    public sealed class PutTeamCommandHandler : PutCommandHandler<long, Domain.Team, DatabaseContext, PutTeamCommand>
    {
        public PutTeamCommandHandler(IDbContextFactory<DatabaseContext> contextFactory, ILogger logger)
            : base(contextFactory, logger)
        {
        }

        protected override async Task BindToDomainEntityAndValidate(
            DatabaseContext context,
            Domain.Team domainEntity,
            PutTeamCommand request,
            CancellationToken cancellationToken)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (domainEntity == null)
            {
                throw new ArgumentNullException(nameof(domainEntity));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var teamsWithSameName = await context
                .Set<Domain.Team>()
                .GetTeamsByName(request.Name)
                .ToArrayAsync(cancellationToken);

            if (teamsWithSameName.Any(i => i.Id != domainEntity.Id))
            {
                var error = new ValidationFailure(nameof(request.Name), string.Format(Domain.Team.ErrorMessages.NameNotUniqueFormat, request.Name));
                this.Logger.Information("Validation failed: a Team with the name {TeamName} already exists", request.Name);
                throw new ValidationException(new ValidationFailure[] { error });
            }

            domainEntity.ChangeName(request.Name);
        }
    }
}
