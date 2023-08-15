namespace Trivia;

public class Player
{
    public Player(string name)
    {
        Name = name;
        Place = 0;
        Purse = 0;
        InPenaltyBox = false;
    }

    public string Name { get; init; }
    public int Place { get; set; }
    public int Purse { get; set; }
    public bool InPenaltyBox { get; set; }
}
