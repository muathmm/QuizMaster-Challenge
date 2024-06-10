using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QuizMaster
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                await QuizMaster();
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
            finally
            {
                Console.WriteLine("The program has completed.");
            }
        }

        public static async Task QuizMaster()
        {
            List<(string Question, string Answer)> quiz = new List<(string, string)>()
    {
      ("What is the keyword to define a structure in C#?", "struct"),
    ("Which keyword is used to define a constant in C#?", "const"),
    ("What is the symbol used for single-line comments in C#?", "//")
    };

            int score = 0;

            try
            {
                foreach (var item in quiz)
                {
                    string userAnswer = null;
                    bool validAnswer = false;
                    bool timedOut = false;

                    Task inputTask = Task.Run(() =>
                    {
                        Console.WriteLine(item.Question);
                        userAnswer = Console.ReadLine();
                        validAnswer = !string.IsNullOrWhiteSpace(userAnswer);
                    });

                    Task timeoutTask = Task.Delay(5000); 

                    Task completedTask = await Task.WhenAny(inputTask, timeoutTask);

                    if (completedTask == timeoutTask)
                    {
                        Console.Clear();
                        Console.WriteLine("Time's up for this question!");
                        Thread.Sleep(3000);
                        Console.Clear();

                        timedOut = true;
                    }
                    else
                    {
                        // User has answered before timeout
                        if (userAnswer.Equals(item.Answer, StringComparison.OrdinalIgnoreCase))
                        {
                            Console.WriteLine("Correct!");
                            await Task.Delay(2000); 
                            Console.Clear();
                            score++;
                        }
                        else
                        {
                            Console.WriteLine($"Incorrect. The correct answer is: {item.Answer}");
                            await Task.Delay(2000); 
                            Console.Clear();
                        }
                    }

                    if (timedOut && !validAnswer)
                    {
                        // Clear input buffer if user didn't enter anything
                        while (Console.KeyAvailable)
                            Console.ReadKey(true);
                    }
                }

                Console.WriteLine($"Your final score is: {score}/{quiz.Count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

    }
}