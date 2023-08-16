﻿using System.Collections.Generic;
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

    public Game(TextWriter writer)
    {
        _writer = writer;

        _questionCategories.Add(new QuestionCategory("Pop"));
        _questionCategories.Add(new QuestionCategory("Science"));
        _questionCategories.Add(new QuestionCategory("Sports"));
        _questionCategories.Add(new QuestionCategory("Rock"));

    }

    public bool IsPlayable()
    {
        return (HowManyPlayers() >= 2);
    }

    public void Add(string playerName)
    {
        _players.Add(new Player(playerName));

        _writer.WriteLine(playerName + " was added");
        _writer.WriteLine("They are player number " + _players.Count);
    }

    public int HowManyPlayers()
    {
        return _players.Count;
    }

    public void Roll(int roll)
    {
        _currentPlayerIndex++;
        if (_currentPlayerIndex == _players.Count) _currentPlayerIndex = 0;
        _currentPlayer = _players[_currentPlayerIndex];

        _writer.WriteLine(_currentPlayer.Name + " is the current player");
        _writer.WriteLine("They have rolled a " + roll);

        if (_currentPlayer.InPenaltyBox)
        {
            //assuming you have to roll odd to get out of penalty box
            if (roll % 2 != 0)
            {
                _currentPlayer.InPenaltyBox = false;

                _writer.WriteLine(_currentPlayer.Name + " is getting out of the penalty box");
                _currentPlayer.MoveSquares(roll);

                _writer.WriteLine(_currentPlayer.Name
                        + "'s new location is "
                        + _currentPlayer.Place);
                _writer.WriteLine("The category is " + CurrentCategory());
                AskQuestion();
            }
            else
            {
                _writer.WriteLine(_currentPlayer.Name + " is not getting out of the penalty box");
            }
        }
        else
        {
            _currentPlayer.MoveSquares(roll);

            _writer.WriteLine(_currentPlayer.Name
                    + "'s new location is "
                    + _currentPlayer.Place);
            _writer.WriteLine("The category is " + CurrentCategory());
            AskQuestion();
        }
    }

    private void AskQuestion()
    {
        _writer.WriteLine(CurrentCategory().PopQuestion());
    }

    private QuestionCategory CurrentCategory()
    {
        return _questionCategories[_currentPlayer.Place % _questionCategories.Count];
    }

    public bool WasCorrectlyAnswered()
    {
        if (_currentPlayer.InPenaltyBox)
        {
            return true;
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

            return !winner;
        }
    }

    public bool WrongAnswer()
    {
        _writer.WriteLine("Question was incorrectly answered");
        _writer.WriteLine(_currentPlayer.Name + " was sent to the penalty box");
        _currentPlayer.InPenaltyBox = true;

        return true;
    }


    private bool DidPlayerWin()
    {
        return _currentPlayer.Purse >= 6;
    }
}
