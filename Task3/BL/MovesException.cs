using System;

namespace Task3.BL
{
    class MovesException : ArgumentException
    {
        public MovesException(string message) : base(message) { }
    }
}
