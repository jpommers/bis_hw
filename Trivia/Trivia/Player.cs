namespace Trivia;

/// <summary>
/// Class representing a player
/// </summary>
public class Player
{
    private const int SquareCount = 12;

    /// <summary>
    /// creates a new player
    /// </summary>
    /// <param name="name">The players name</param>
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

    /// <summary>
    /// Moves player on the board
    /// </summary>
    /// <param name="roll">How many squares to move</param>
    public void MoveSquares(int roll)
    {
        Place += roll;
        if (Place >= SquareCount) Place -= SquareCount;
    }
}
