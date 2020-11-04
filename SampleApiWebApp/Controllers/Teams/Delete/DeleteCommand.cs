using AspNetCoreApi.Infrastructure.Mediation;

namespace SampleApiWebApp.Controllers.Teams.Delete
{
    public class DeleteCommand : IDeleteCommand<long>
    {
        public long Id { get; set; }
    }
}
