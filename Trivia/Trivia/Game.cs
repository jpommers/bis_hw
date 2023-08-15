using System;
using System.Collections.Generic;
using System.IO;

namespace Trivia;

public class Game
{
    private const int SquareCount = 12;

    private readonly TextWriter _writer;

    private readonly List<Player> _players = new();

    private readonly LinkedList<string> _popQuestions = new();
    private readonly LinkedList<string> _scienceQuestions = new();
    private readonly LinkedList<string> _sportsQuestions = new();
    private readonly LinkedList<string> _rockQuestions = new();

    private int _currentPlayerIndex;
    private Player _currentPlayer;
    private bool _isGettingOutOfPenaltyBox;

    public Game(TextWriter writer)
    {
        _writer = writer;

        for (var i = 0; i < 50; i++)
        {
            _popQuestions.AddLast("Pop Question " + i);
            _scienceQuestions.AddLast(("Science Question " + i));
            _sportsQuestions.AddLast(("Sports Question " + i));
            _rockQuestions.AddLast("Rock Question " + i);
        }
    }

    public bool IsPlayable()
    {
        return (HowManyPlayers() >= 2);
    }

    public bool Add(string playerName)
    {
        _players.Add(new Player(playerName));

        _writer.WriteLine(playerName + " was added");
        _writer.WriteLine("They are player number " + _players.Count);
        return true;
    }

    public int HowManyPlayers()
    {
        return _players.Count;
    }

    public void Roll(int roll)
    {
        _currentPlayer = _players[_currentPlayerIndex];

        _writer.WriteLine(_currentPlayer.Name + " is the current player");
        _writer.WriteLine("They have rolled a " + roll);

        if (_currentPlayer.InPenaltyBox)
        {
            if (roll % 2 != 0)
            {
                _isGettingOutOfPenaltyBox = true;

                _writer.WriteLine(_currentPlayer.Name + " is getting out of the penalty box");
                _currentPlayer.Place = _currentPlayer.Place + roll;
                if (_currentPlayer.Place == SquareCount) _currentPlayer.Place = _currentPlayer.Place - SquareCount;

                _writer.WriteLine(_currentPlayer.Name
                        + "'s new location is "
                        + _currentPlayer.Place);
                _writer.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
            else
            {
                _writer.WriteLine(_currentPlayer.Name + " is not getting out of the penalty box");
                _isGettingOutOfPenaltyBox = false;
            }
        }
        else
        {
            _currentPlayer.Place = _currentPlayer.Place + roll;
            if (_currentPlayer.Place == SquareCount) _currentPlayer.Place = _currentPlayer.Place - SquareCount;

            _writer.WriteLine(_currentPlayer.Name
                    + "'s new location is "
                    + _currentPlayer.Place);
            _writer.WriteLine("The category is " + CurrentCategory());
            AskQuestion();
        }
    }

    private void AskQuestion()
    {
        if (CurrentCategory() == "Pop")
        {
            _writer.WriteLine(_popQuestions.First.Value);
            _popQuestions.RemoveFirst();
        }
        if (CurrentCategory() == "Science")
        {
            _writer.WriteLine(_scienceQuestions.First.Value);
            _scienceQuestions.RemoveFirst();
        }
        if (CurrentCategory() == "Sports")
        {
            _writer.WriteLine(_sportsQuestions.First.Value);
            _sportsQuestions.RemoveFirst();
        }
        if (CurrentCategory() == "Rock")
        {
            _writer.WriteLine(_rockQuestions.First.Value);
            _rockQuestions.RemoveFirst();
        }
    }

    private string CurrentCategory()
    {
        if (_currentPlayer.Place == 0) return "Pop";
        if (_currentPlayer.Place == 4) return "Pop";
        if (_currentPlayer.Place == 8) return "Pop";

        if (_currentPlayer.Place == 1) return "Science";
        if (_currentPlayer.Place == 5) return "Science";
        if (_currentPlayer.Place == 9) return "Science";

        if (_currentPlayer.Place == 2) return "Sports";
        if (_currentPlayer.Place == 6) return "Sports";
        if (_currentPlayer.Place == 10) return "Sports";

        return "Rock";
    }

    public bool WasCorrectlyAnswered()
    {
        if (_currentPlayer.InPenaltyBox)
        {
            if (_isGettingOutOfPenaltyBox)
            {
                _writer.WriteLine("Answer was correct!!!!");
                _currentPlayer.Purse++;
                _writer.WriteLine(_currentPlayer.Name
                        + " now has "
                        + _currentPlayer.Purse
                        + " Gold Coins.");

                var winner = DidPlayerWin();
                _currentPlayerIndex++;
                if (_currentPlayerIndex == _players.Count) _currentPlayerIndex = 0;

                return !winner;
            }
            else
            {
                _currentPlayerIndex++;
                if (_currentPlayerIndex == _players.Count) _currentPlayerIndex = 0;
                return true;
            }
        }
        else
        {
            _writer.WriteLine("Answer was corrent!!!!");
            _currentPlayer.Purse++;
            _writer.WriteLine(_currentPlayer.Name
                    + " now has "
                    + _currentPlayer.Purse
                    + " Gold Coins.");

            var winner = DidPlayerWin();
            _currentPlayerIndex++;
            if (_currentPlayerIndex == _players.Count) _currentPlayerIndex = 0;

            return !winner;
        }
    }

    public bool WrongAnswer()
    {
        _writer.WriteLine("Question was incorrectly answered");
        _writer.WriteLine(_currentPlayer.Name + " was sent to the penalty box");
        _currentPlayer.InPenaltyBox = true;

        _currentPlayerIndex++;
        if (_currentPlayerIndex == _players.Count) _currentPlayerIndex = 0;
        return true;
    }


    private bool DidPlayerWin()
    {
        return _currentPlayer.Purse >= 6;
    }
}
