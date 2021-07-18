using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Task3.BL
{
    class Game
    {
        private string[] _moves;
        private byte[] _key = new Byte[16];
        private int _computerInput;
        private byte[] _hmac;

        public string[] Moves { 
            get
            {
                return _moves;
            }
        }

        public string Key
        {
            get
            {
                return ConvertByteArrayToHex(_key);
            }
        }

        public string Hmac
        {
            get
            {
                return ConvertByteArrayToHex(_hmac);
            }
        }
        public string ComputerInput
        {
            get
            {
                return _moves[_computerInput];
            }
        }

        private void ValidateMoves(string[] moves)
        {
            string message = "";
            if (moves.Distinct().Count() != moves.Length)
            {
                message += "There must be non-repeating strings.";
            }
            if (moves.Length % 2 == 0 || moves.Length < 3)
            {
                message += "You need to transfer an odd number> = 3.";
            }
            if (message.Length > 0)
            {
                message += "\nCorrect input: rock paper scissors lizard Spock";
                throw new MovesException(message);
            }
        }

        public Game(string[] moves)
        {
            ValidateMoves(moves);
            _moves = moves;
        }

        public void Start()
        {
            GenerateKey();
            GenerateComputerInput();
            ComputeHMAC();
        }

        private void GenerateKey()
        {
            RandomNumberGenerator.Fill(_key);
        }

        private void GenerateComputerInput()
        {
            _computerInput  = RandomNumberGenerator.GetInt32(0, _moves.Length);
        }

        private void ComputeHMAC()
        {
            var hmac = new HMACSHA256(_key);
            try
            {
                _hmac = hmac.ComputeHash(Encoding.UTF8.GetBytes(_moves[_computerInput]));
            }
            finally
            {
                if (hmac != null)
                {
                    ((IDisposable)hmac).Dispose();
                }
            }
        }

        public GameResult Compete(int userInput)
        {
            if (userInput == _computerInput)
            {
                return GameResult.Draw;
            }
            for (int i = 1; i <= _moves.Length / 2; i++)
            {
                int a = userInput + i;
                if (a >= _moves.Length)
                {
                    a -= _moves.Length;
                }
                if (_moves[a] == _moves[_computerInput])
                {
                    return GameResult.Lose;
                }
            }
            return GameResult.Win;
        }

        private string ConvertByteArrayToHex(byte[] arr)
        {
            return BitConverter.ToString(arr).Replace("-", "");
        }
    }
}
