﻿
namespace CKK.Logic.Exceptions
{
    public class InvalidIdException : Exception
    {
        public InvalidIdException() : base() { }
        public InvalidIdException(string message) : base(message) { }
        public InvalidIdException(string message, Exception innerException) : base(message, innerException) { }
    }
}
