using FluentAssertions;
using System;
using System.IO;
using System.Xml.Linq;
using Trivia;
using Xunit;

namespace Tests;

public class GameTests
{
    private readonly StringWriter _stringWriter;
    private readonly Game _game;

    public GameTests()
    {
        _stringWriter= new StringWriter();
        _game = new Game(_stringWriter);

        _game.Add("Janis");
        _game.Add("Peter");
    }

    [Fact]
    public void WhenAllAnswerCorrectly_FirstPlayerShouldWin()
    {
        //ARRANGE
        var expectedOutput = File.ReadAllText($"ExpectedOutput/{nameof(WhenAllAnswerCorrectly_FirstPlayerShouldWin)}.txt");

        //ACT
        bool _notAWinner;
        do
        {
            _game.Roll(1);

            _notAWinner = _game.WasCorrectlyAnswered();

        } while (_notAWinner);

        //ASSERT
        _stringWriter.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public void WhenPlayerRollsOdd_ShouldGetOutOfPenaltyBox()
    {
        //ARRANGE
        var expectedOutput = File.ReadAllText($"ExpectedOutput/{nameof(WhenPlayerRollsOdd_ShouldGetOutOfPenaltyBox)}.txt");

        //ACT

        //player 1 goes to penalty box
        _game.Roll(2);
        _game.WrongAnswer();

        //player 2 answers correctly
        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        //player 1 gets out of penalty box
        _game.Roll(1);
        _game.WasCorrectlyAnswered();

        //player 2 answers correctly
        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        //player 1 rolls even, but is not in penalty box, so should be allowed to answer
        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        //ASSERT
        _stringWriter.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public void WhenPlayerGoesOverSquareCount_LocationShouldLoop()
    {
        //ARRANGE
        var expectedOutput = File.ReadAllText($"ExpectedOutput/{nameof(WhenPlayerGoesOverSquareCount_LocationShouldLoop)}.txt");

        //ACT

        //player 1 rolls 6
        _game.Roll(6);
        _game.WasCorrectlyAnswered();

        //player 2 rolls 6
        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        //player 1 rolls 6
        _game.Roll(6);
        _game.WasCorrectlyAnswered();

        //player 2 rolls 6
        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        //player 1 rolls 6
        _game.Roll(6);
        _game.WasCorrectlyAnswered();

        //player 2 rolls 6
        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        //ASSERT
        _stringWriter.ToString().Should().Be(expectedOutput);
    }

    [Fact]
    public void WhenPlayerInPenaltyBox_ShouldNotBeAbleToGetPoints()
    {
        //ARRANGE
        var expectedOutput = File.ReadAllText($"ExpectedOutput/{nameof(WhenPlayerInPenaltyBox_ShouldNotBeAbleToGetPoints)}.txt");

        //ACT

        //player 1 answer wrongly
        _game.Roll(1);
        _game.WrongAnswer();

        //player 2 answers right
        _game.Roll(1);
        _game.WasCorrectlyAnswered();

        //player 1 rolls 2 (does not get out of jail)
        _game.Roll(2);
        _game.WasCorrectlyAnswered();

        //ASSERT
        _stringWriter.ToString().Should().Be(expectedOutput);
    }
}
