using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
       
        string inputFile = @"C:\\Users\\user\\OneDrive\\Masaüstü\\booklet\\sample.txt";
        int numBooklets = 3;
        int questionsPerBooklet = 5;

        try
        {
            List<Question> questions = ReadInputFile(inputFile);
            int totalQuestions = questions.Count;

            Console.WriteLine($"Total questions in the input file: {totalQuestions}");

            if (totalQuestions < numBooklets * questionsPerBooklet)
            {
                Console.WriteLine($"Not enough questions in the input file.");
                Console.WriteLine($"Total questions required: {numBooklets * questionsPerBooklet}");
                return;
            }

            List<List<Question>> allBooklets = GenerateBooklets(questions, numBooklets, questionsPerBooklet);

            for (int i = 0; i < numBooklets; i++)
            {
                Console.WriteLine($"\nBooklet {i + 1} - Questions:");
                PrintQuestionsToConsole(allBooklets[i]);

                Console.WriteLine($"\nBooklet {i + 1} - Answer Sheet:");
                PrintAnswerSheetToConsole(allBooklets[i]);
            }
        }
        catch (IOException e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }
    }

    static List<Question> ReadInputFile(string filePath)
    {
        List<Question> questions = new List<Question>();
        string[] lines = File.ReadAllLines(filePath);

        for (int i = 0; i < lines.Length; i++)
        {
            if (lines[i].StartsWith("Q") && lines[i].Contains(":"))
            {
                
                string question = lines[i];
                string answer = lines[i + 1].Trim();
                List<string> options = new List<string>();

                for (int j = i + 2; j < lines.Length; j++)
                {
                    if (string.IsNullOrWhiteSpace(lines[j]))
                    {
                        i = j;
                        break;
                    }
                    options.Add(lines[j].Trim());
                }

                questions.Add(new Question(question,answer, options));
            }
            else if (!string.IsNullOrWhiteSpace(lines[i]))
            {
                Console.WriteLine($"Invalid input line: {lines[i]}");
            }
        }

        return questions;
    }

    static List<List<Question>> GenerateBooklets(List<Question> questions, int numBooklets, int questionsPerBooklet)
    {
        List<List<Question>> allBooklets = new List<List<Question>>();

        List<Question> allQuestions = new List<Question>(questions);

        for (int i = 0; i < numBooklets; i++)
        {
            
            Random rand = new Random();
            List<Question> bookletQuestions = allQuestions.OrderBy(q => rand.Next()).Take(questionsPerBooklet).ToList();

            foreach (Question question in bookletQuestions)
            {
                question.ShuffleOptions();
            }

            
            allBooklets.Add(bookletQuestions);
        }

        return allBooklets;
    }

    static void PrintQuestionsToConsole(List<Question> questions)
    {
        foreach (Question question in questions)
        {
            Console.WriteLine($"{question.Text}");
            Console.WriteLine($"Options: {string.Join(", ", question.Options)}\n");
        }
    }

    static void PrintAnswerSheetToConsole(List<Question> questions)
    {
        foreach (Question question in questions)
        {
            Console.WriteLine($"{question.Text}");
            Console.WriteLine($"Correct Answer: {question.Answer}\n");
        }
    }
}

class Question
{
    public string Text { get; }
    public string Answer { get; }
    public List<string> Options { get; private set; }

    public Question(string text, string answer, List<string> options)
    {
        Text = text;
        Answer = answer;
        Options = options;
    }

    public void ShuffleOptions()
    {
        Random rand = new Random();
        Options = Options.OrderBy(o => rand.Next()).ToList();
    }
}
