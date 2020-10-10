using SampleApiWebApp.Domain.Validators;

namespace SampleApiWebApp.Controllers.Teams.Put
{
    public sealed class PutTeamCommandValidator : BaseTeamValidator<PutTeamCommand>
    {
        public PutTeamCommandValidator()
            : base()
        {
        }
    }
}
