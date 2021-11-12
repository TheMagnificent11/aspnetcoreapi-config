namespace Sample.Domain;

public interface IPlayer
{
    string GivenName { get; }

    string Surname { get; }

    Guid TeamId { get; }

    int Number { get; }
}
