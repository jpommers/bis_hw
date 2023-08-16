using System.Collections.Generic;

namespace Trivia;

/// <summary>
/// Class representing a question category
/// </summary>
public class QuestionCategory
{
    private LinkedList<string> Questions { get; init; } = new LinkedList<string>();

    /// <summary>
    /// Creates a new question category instance
    /// </summary>
    /// <param name="name">Category name</param>
    public QuestionCategory(string name)
    {
        Name = name;

        for (var i = 0; i < 50; i++)
        {
            Questions.AddLast($"{name} Question " + i);
        }
    }

    public string Name { get; init; }

    /// <summary>
    /// Returns next question
    /// </summary>
    /// <returns>Next question</returns>
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
