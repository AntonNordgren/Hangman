using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hangman
{
    static class Game
    {
        private static string path;
        private static bool programRunning;
        private static string playAgain;
        private static string theWord;
        private static char[] hiddenWord;
        private static int guessesLeft;
        private static string userInput;
        private static StringBuilder wrongGuesses;
        private static List<string> listOfGuesses;

        public static void Init()
        {
            path = "words.txt";
            programRunning = true;
            theWord = GenerateWord(path);
            hiddenWord = new char[theWord.Length];

            for (int i = 0; i < hiddenWord.Length; i++)
            {
                hiddenWord[i] = '_';
            }

            guessesLeft = 10;
            wrongGuesses = new StringBuilder();
            listOfGuesses = new List<string>();
            Start();

        }
        private static string GenerateWord(string filePath)
        {
            Random rnd = new Random();
            List<string> lines = File.ReadAllText(filePath).Split(",").ToList();

            return lines[rnd.Next(lines.Count)].ToUpper();
        }

        private static void PrintGameState()
        {
            Console.Clear();
            Console.WriteLine("Hangman game");
            Console.WriteLine();

            Console.Write("Wrong guesses: ");
            Console.Write(wrongGuesses.ToString());

            Console.WriteLine();
            Console.WriteLine("Guesses left: " + guessesLeft);
            Console.WriteLine();

            Console.Write("Hidden word: ");
            foreach (char c in hiddenWord)
            {
                Console.Write(c);
            }

            Console.WriteLine();
            Console.WriteLine();
        }

        private static bool CheckInput(string s)
        {

            if (listOfGuesses.Contains(s))
            {
                return true;
            }

            return false;
        }

        private static bool FoundWordCheck(char[] c)
        {
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == '_')
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetUserString()
        {
            string input;

            while (true)
            {
                Console.Write("Guess a character or word: ");
                input = Console.ReadLine();

                if (input != "" && input.All(Char.IsLetter))
                {
                    return input.ToUpper();
                }
                else
                {
                    Console.WriteLine("Invalid input, try again");
                }

            }
        }

        private static void Reset()
        {
            programRunning = true;
            theWord = GenerateWord(path);
            hiddenWord = new char[theWord.Length];

            for (int i = 0; i < hiddenWord.Length; i++)
            {
                hiddenWord[i] = '_';
            }

            guessesLeft = 10;
            wrongGuesses = new StringBuilder();
            listOfGuesses = new List<string>();
        }

        private static void Start()
        {
            while (programRunning)
            {
                while (true)
                {
                    if (guessesLeft == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Game Over!");
                        Console.WriteLine("You have no guesses left!");
                        Console.WriteLine("The right word was: " + theWord);
                        Console.WriteLine();
                        break;
                    }

                    PrintGameState();
                    userInput = GetUserString();

                    if (!CheckInput(userInput))
                    {
                        listOfGuesses.Add(userInput);

                        if (userInput.Length == 1)
                        {
                            if (theWord.Contains(userInput))
                            {

                                for (int i = 0; i < hiddenWord.Length; i++)
                                {
                                    if (theWord[i] == userInput.ToCharArray()[0])
                                    {
                                        hiddenWord[i] = userInput.ToCharArray()[0];
                                    }
                                }

                                guessesLeft--;
                            }
                            else
                            {
                                wrongGuesses.Append(userInput + ", ");
                                guessesLeft--;
                            }

                            if (FoundWordCheck(hiddenWord))
                            {
                                Console.WriteLine("You found the word!");
                                Console.WriteLine("The right word was: " + theWord);
                                Console.WriteLine();
                                break;
                            }

                        }
                        else
                        {
                            if (userInput.Length != theWord.Length)
                            {
                                Console.WriteLine("Your guess is not the same length as the hidden word");
                                Console.WriteLine();
                                Console.Write("Enter any key to continue: ");
                                Console.ReadKey();
                            }
                            else
                            {
                                if (userInput == theWord)
                                {
                                    Console.WriteLine("You found the word!");
                                    Console.WriteLine("The right word was: " + theWord);
                                    Console.WriteLine();
                                    break;
                                }
                                else
                                {
                                    wrongGuesses.Append(userInput + ", ");
                                    guessesLeft--;
                                }
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine();
                        Console.WriteLine("You've already guessed that.");
                        Console.Write("Enter any key to continue: ");
                        Console.ReadKey();
                    }
                }

                while (true)
                {
                    Console.Write("Do you want to play again?: (y for yes or n for no): ");
                    playAgain = Console.ReadLine();

                    if (playAgain == "y" || playAgain == "n")
                    {
                        if (playAgain == "n")
                        {
                            programRunning = false;
                            Console.WriteLine("Exiting...");
                            break;
                        }
                        else if (playAgain == "y")
                        {
                            Reset();
                            break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input");
                    }

                }
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Game.Init();
        }
    }
}
