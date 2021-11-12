using EntityManagement.Core;
using FluentValidation;
using Sample.Domain.Validators;

namespace Sample.Domain;

public class Player : BaseEntity<Guid>, IPlayer
{
    private Player(string givenName, string surname, Team team, int number)
    {
        this.GivenName = givenName;
        this.Surname = surname;
        this.Team = team;
        this.TeamId = team.Id;
        this.Number = number;
    }

    public string GivenName { get; protected set; }

    public string Surname { get; protected set; }

    public Guid TeamId { get; protected set; }

    public Team Team { get; protected set; }

    public int Number { get; protected set; }

    public void ChangeTeams(Team newTeam, int number)
    {
        if (newTeam is null)
        {
            throw new ArgumentNullException(nameof(newTeam));
        }

        this.Team.RemovePlayer(this);

        newTeam.AddPlayer(this, number);

        this.TeamId = newTeam.Id;
        this.Team = newTeam;

        Validate(this);
    }

    internal static Player Create(string givenName, string surname, Team team, int number)
    {
        if (givenName is null)
        {
            throw new ArgumentNullException(nameof(givenName));
        }

        if (surname is null)
        {
            throw new ArgumentNullException(nameof(surname));
        }

        if (team is null)
        {
            throw new ArgumentNullException(nameof(team));
        }

        var player = new Player(givenName, surname, team, number);

        Validate(player);

        return player;
    }

    private static void Validate(Player player)
    {
        var validator = new PlayerValidator();
        var result = validator.Validate(player);

        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }

    public static class FieldNameMaxLengths
    {
        public const int Name = 50;
    }
}