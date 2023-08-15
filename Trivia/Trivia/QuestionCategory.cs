using System.Collections.Generic;

namespace Trivia;

public class QuestionCategory
{
    private LinkedList<string> Questions { get; init; } = new LinkedList<string>();

    public QuestionCategory(string name)
    {
        Name = name;

        for (var i = 0; i < 50; i++)
        {
            Questions.AddLast($"{name} Question " + i);
        }
    }

    public string Name { get; init; }

    public string PopQuestion()
    {
        var question = Questions.First.Value;
        Questions.RemoveFirst();
        return question;
    }

    public override string ToString()
    {
        return Name;
    }
}
