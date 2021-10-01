using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Hangman
{
    static class Game
    {
        private static bool programRunning;
        private static string playAgain;
        private static string theWord;
        private static char[] hiddenWord;
        private static int guessesLeft;
        private static string userInput;
        private static StringBuilder wrongGuesses;
        private static List<string> listOfGuesses;

        private static void readFromFile()
        {
            string filePath = "words.txt";
            List<string> lines = File.ReadAllLines(filePath).ToList();
            List<string> word = new List<string>();

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
        }

        public static void Init()
        {
            programRunning = true;
            theWord = GenerateWord("words.txt");
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
        private static string GenerateWord(string path)
        {
            Random rnd = new Random();
            List<string> lines = File.ReadAllText(path).Split(", ").ToList();

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
                Console.Write(c + "");
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

                if(input != "" && input.All(Char.IsLetter))
                {
                    return input.ToUpper();
                }
                else
                {
                    Console.WriteLine("Invalid input, try again");
                }

            }
        }

        private static void reset()
        {
            programRunning = true;
            theWord = GenerateWord("words.txt");
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
            while(programRunning)
            {
                while (true)
                {
                    if (guessesLeft == 0)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Game Over!");
                        Console.WriteLine("You have no guesses left!");
                        Console.WriteLine("The right word was: " + theWord);

                        Console.Write("Enter any key to continue: ");
                        Console.ReadKey();
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

                            }
                            else
                            {
                                wrongGuesses.Append(userInput + ", ");
                                Console.WriteLine("The character is not in the word.");
                                Console.WriteLine("Enter any key to continue: ");
                                Console.ReadKey();
                            }

                            if(FoundWordCheck(hiddenWord))
                            {
                                Console.WriteLine("You found the word!");
                                Console.WriteLine("The right word was: " + theWord);
                                Console.ReadKey();
                                break;
                            }
                    
                        }
                        else
                        {
                            if(userInput == theWord )
                            {
                                Console.WriteLine("You found the word!");
                                Console.WriteLine("Enter any key to continue: ");
                                Console.ReadKey();
                                break;
                            }
                            else
                            {
                                wrongGuesses.Append(userInput + ", ");
                                Console.WriteLine("Wrong word");
                                Console.Write("Enter any key to continue: ");
                                Console.ReadKey();
                            }

                        }

                        guessesLeft--;
                    }
                    else
                    {
                        Console.WriteLine("You've already guessed that.");
                        Console.WriteLine("Enter any key to continue: ");
                        Console.ReadKey();
                    }
                }

                while(true)
                {
                    Console.Write("Do you want to play again?: (y for yes or n for no): ");
                    playAgain = Console.ReadLine();

                    if(playAgain == "y" || playAgain == "n")
                    {
                        if (playAgain == "n")
                        {
                            programRunning = false;
                            break;
                        }
                        else if (playAgain == "y")
                        {
                            reset();
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
