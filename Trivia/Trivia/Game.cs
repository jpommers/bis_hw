using System.Collections.Generic;
using System.IO;

namespace Trivia;

/// <summary>
/// Game rules:
/// * Each player rolls a dice and ends up on a square.
/// * Each square has its own Trivia category
/// * Answering question correctly gives you a point
/// * Answering wrongly sends you to penalty box
/// * You get out of penalty box by rolling an odd number
/// * If you roll even while in jail you skip turn
/// * You can answer after getting out of penalty box
/// * First player to get to 6 points wins
/// </summary>
public class Game
{
    private readonly TextWriter _writer;

    private readonly List<Player> _players = new();

    private readonly List<QuestionCategory> _questionCategories = new();

    private int _currentPlayerIndex = -1;
    private Player _currentPlayer;

    /// <summary>
    /// Creates a new game instance.
    /// </summary>
    /// <param name="writer">TextWriter to which the game writes its output.</param>
    public Game(TextWriter writer)
    {
        _writer = writer;

        _questionCategories.Add(new QuestionCategory("Pop"));
        _questionCategories.Add(new QuestionCategory("Science"));
        _questionCategories.Add(new QuestionCategory("Sports"));
        _questionCategories.Add(new QuestionCategory("Rock"));

    }

    /// <summary>
    /// Is the game playable
    /// </summary>
    /// <returns>True if the game can be played with the amount of players</returns>
    public bool IsPlayable()
    {
        return (HowManyPlayers() >= 2);
    }

    /// <summary>
    /// Adds a player to the game
    /// </summary>
    /// <param name="playerName">Name of the player</param>
    public void Add(string playerName)
    {
        _players.Add(new Player(playerName));

        _writer.WriteLine($"{playerName} was added");
        _writer.WriteLine($"They are player number {_players.Count}");
    }

    /// <summary>
    /// How many players there are in the game
    /// </summary>
    /// <returns>The amount of players</returns>
    public int HowManyPlayers()
    {
        return _players.Count;
    }

    /// <summary>
    /// Rolls a dice and executes a players turn
    /// </summary>
    /// <param name="roll">The number on the dice rolled</param>
    public void Roll(int roll)
    {
        //advances and loops the current player
        _currentPlayerIndex++;
        if (_currentPlayerIndex == _players.Count) _currentPlayerIndex = 0;
        _currentPlayer = _players[_currentPlayerIndex];

        _writer.WriteLine($"{_currentPlayer} is the current player");
        _writer.WriteLine($"They have rolled a {roll}");

        //release the player from penalty box if needed
        if (_currentPlayer.InPenaltyBox)
        {
            //assuming you have to roll odd to get out of penalty box
            if (roll % 2 != 0)
            {
                _currentPlayer.InPenaltyBox = false;

                _writer.WriteLine($"{_currentPlayer} is getting out of the penalty box");
            }
            else
            {
                _writer.WriteLine($"{_currentPlayer} is not getting out of the penalty box");
            }
        }

        if(!_currentPlayer.InPenaltyBox)
        {
            _currentPlayer.MoveSquares(roll);

            _writer.WriteLine($"{_currentPlayer}'s new location is {_currentPlayer.Place}");
            _writer.WriteLine($"The category is {CurrentCategory()}");
            AskQuestion();
        }
    }

    /// <summary>
    /// Asks(writes to the output) a question
    /// </summary>
    private void AskQuestion()
    {
        _writer.WriteLine(CurrentCategory().PopQuestion());
    }

    /// <summary>
    /// Returns the current category
    /// </summary>
    /// <returns>Instance of current category</returns>
    private QuestionCategory CurrentCategory()
    {
        //categories are assigned to each square in order and in loop
        //if there are more categories than squares then the last categories are never assigned
        return _questionCategories[_currentPlayer.Place % _questionCategories.Count];
    }

    /// <summary>
    /// Specifies that the player answered correctly
    /// </summary>
    /// <returns>Should the game continue</returns>
    public bool WasCorrectlyAnswered()
    {
        if (_currentPlayer.InPenaltyBox) return true;

        _writer.WriteLine("Answer was corrent!!!!");
        _currentPlayer.Purse++;
        _writer.WriteLine($"{_currentPlayer} now has {_currentPlayer.Purse} Gold Coins.");

        return !DidPlayerWin();
    }

    /// <summary>
    /// Specifies that the player answered wrongly
    /// </summary>
    /// <returns>Should the game continue</returns>
    public bool WrongAnswer()
    {
        _writer.WriteLine("Question was incorrectly answered");
        _writer.WriteLine($"{_currentPlayer} was sent to the penalty box");
        _currentPlayer.InPenaltyBox = true;

        return true;
    }


    /// <summary>
    /// Tells if current player has won
    /// </summary>
    /// <returns>If current player has won</returns>
    private bool DidPlayerWin()
    {
        return _currentPlayer.Purse >= 6;
    }
}
