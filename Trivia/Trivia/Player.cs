namespace Trivia;

public class Player
{
    private const int SquareCount = 12;

    public Player(string name)
    {
        Name = name;
        Place = 0;
        Purse = 0;
        InPenaltyBox = false;
    }

    public string Name { get; init; }
    public int Place { get; private set; }
    public int Purse { get; set; }
    public bool InPenaltyBox { get; set; }

    public void MoveSquares(int roll)
    {
        Place = Place + roll;
        if (Place == SquareCount) Place = Place - SquareCount;
    }
}
