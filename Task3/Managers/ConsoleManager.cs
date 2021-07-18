using System;
using System.Linq;
using Task3.BL;

namespace Task3.Managers
{
    class ConsoleManager
    {
        private Game _game;
        private int _userInput;

        private void ShowMenu()
        {
            Console.WriteLine("Available moves:");
            for (var i = 0; i < _game.Moves.Length; i++)
            {
                Console.WriteLine($"{i + 1} - {_game.Moves[i]}");
            }
            Console.WriteLine("0 - exit");
        }

        private void GetUserInput()
        {
            while (true)
            {
                Console.Write("Enter your move: ");
                try
                {
                    _userInput = Convert.ToInt32(Console.ReadLine());
                    if (_userInput > _game.Moves.Length || _userInput < 0)
                    {
                        throw new IndexOutOfRangeException("Wrong input. Try again...");
                    }
                    return;
                } 
                catch (IndexOutOfRangeException e)
                {
                    Console.WriteLine(e.Message);
                    ShowMenu();
                }
            }
        }

        private void ShowCompetitionResult()
        {
            switch (_game.Compete(_userInput))
            {
                case GameResult.Win:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("You win!");
                    break;
                case GameResult.Lose:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("You lose!");
                    break;
                case GameResult.Draw:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("It's a draw!");
                    break;
            }
            Console.ResetColor();
        }

        public void Start(string[] moves)
        {
            try
            {
                _game = new Game(moves);
            }
            catch (MovesException e)
            {
                Console.WriteLine($"\a{e.Message}.\nPress any key to exit...");
                Console.ReadKey();
                return;
            }
            while (true)
            {
                _game.Start();
                Console.WriteLine($"HMAC: {_game.Hmac}");
                ShowMenu();
                GetUserInput();
                var move = _game.Moves.ElementAtOrDefault(_userInput - 1);
                Console.WriteLine("Your move: {0}", String.IsNullOrEmpty(move) ? "exit" : move);
                if (_userInput == 0) break;
                Console.WriteLine($"Computer move: {_game.ComputerInput}");
                ShowCompetitionResult();
                Console.WriteLine($"HMAC key: {_game.Key}");
            }
        }
    }
}
