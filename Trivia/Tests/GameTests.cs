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
}
