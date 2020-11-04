using AspNetCoreApi.Infrastructure.Mediation;
using EntityManagement;
using Serilog;

namespace SampleApiWebApp.Controllers.Teams.Delete
{
    public sealed class DeleteCommandHandler : DeleteCommandHandler<long, Domain.Team, DeleteCommand>
    {
        public DeleteCommandHandler(IDatabaseContext databaseContext, ILogger logger)
            : base(databaseContext, logger)
        {
        }

        protected override void DeleteDomainEntity(Domain.Team domainEntity)
        {
            this.DatabaseContext.EntitySet<Domain.Team>().Remove(domainEntity);
        }
    }
}
