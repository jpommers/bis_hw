using System;

namespace Trivia;

public class GameRunner
{
    private static bool _shouldGameContinue;

    public static void Main()
    {
        var game = new Game(Console.Out);

        game.Add("Chet");
        game.Add("Pat");
        game.Add("Sue");

        var rand = new Random();

        do
        {
            game.Roll(rand.Next(5) + 1);

            if (rand.Next(9) == 7)
            {
                _shouldGameContinue = game.WrongAnswer();
            }
            else
            {
                _shouldGameContinue = game.WasCorrectlyAnswered();
            }
        } while (_shouldGameContinue);
    }
}