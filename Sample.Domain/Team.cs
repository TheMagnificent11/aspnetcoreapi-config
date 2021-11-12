using EntityManagement.Core;
using FluentValidation;
using FluentValidation.Results;
using Sample.Domain.Validators;

namespace Sample.Domain;

public class Team : BaseEntity<Guid>, ITeam
{
    private readonly List<Player> players = new ();

    public string Name { get; protected set; }

    public IReadOnlyCollection<Player> Players => this.players;

    public static Team CreateTeam(string teamName)
    {
        if (teamName is null)
        {
            throw new ArgumentNullException(nameof(teamName));
        }

        var team = new Team { Id = Guid.NewGuid(), Name = teamName };

        team.ApplyTrackingData();

        ValidateTeam(team);

        return team;
    }

    public static void ValidateTeam(Team team)
    {
        var validator = new TeamValidator();

        var result = validator.Validate(team);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }

    public void ChangeName(string newName)
    {
        this.Name = newName;

        this.ApplyTrackingData();

        ValidateTeam(this);
    }

    public void AddPlayer(string playerGivenName, string playerSurname, int number)
    {
        if (playerGivenName is null)
        {
            throw new ArgumentNullException(nameof(playerGivenName));
        }

        if (playerSurname is null)
        {
            throw new ArgumentNullException(nameof(playerSurname));
        }

        this.ValidatePlayerNumber(number);

        var player = Player.Create(playerGivenName, playerSurname, this, number);

        this.players.Add(player);
    }

    internal void AddPlayer(Player player, int number)
    {
        if (player is null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        this.ValidatePlayerNumber(number);

        this.players.Add(player);
    }

    internal void RemovePlayer(Player player)
    {
        if (player is null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        this.players.Remove(player);
    }

    private void ValidatePlayerNumber(int number)
    {
        if (this.IsNumberTaken(number))
        {
            var failure = new ValidationFailure(
                nameof(Player.Number),
                "Number is already taken by another player in the team",
                number);

            throw new ValidationException(new ValidationFailure[] { failure });
        }
    }

    private bool IsNumberTaken(int number)
    {
        return this.Players
            .Select(p => p.Number)
            .Any(n => n == number);
    }

    public static class FieldMaxLenghts
    {
        public const int Name = 50;
    }

    public static class ErrorMessages
    {
        public const string NameNotUniqueFormat = "Team Name '{0}' is not unqiue";
    }
}
