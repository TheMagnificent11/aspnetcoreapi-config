using Sample.Domain;

namespace Sample.Application.Players;

public class Player : IPlayer
{
    public Guid Id { get; set; }

    public string GivenName { get; set; }

    public string Surname { get; set; }

    public Guid TeamId { get; set; }

    public int Number { get; set; }
}
