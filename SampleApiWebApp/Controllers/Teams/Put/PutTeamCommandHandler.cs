using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreApi.Infrastructure.Mediation;
using EntityManagement;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using SampleApiWebApp.Data.Queries;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.Put
{
    public sealed class PutTeamCommandHandler : PutCommandHandler<long, Domain.Team, PutTeamCommand>
    {
        public PutTeamCommandHandler(IDatabaseContext databaseContext, ILogger logger)
            : base(databaseContext, logger)
        {
        }

        protected override async Task BindToDomainEntityAndValidate(
            [NotNull] Domain.Team domainEntity,
            [NotNull] PutTeamCommand request,
            [NotNull] CancellationToken cancellationToken)
        {
            if (domainEntity == null)
            {
                throw new ArgumentNullException(nameof(domainEntity));
            }

            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var teamsWithSameName = await this.DatabaseContext
                .EntitySet<Domain.Team>()
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
