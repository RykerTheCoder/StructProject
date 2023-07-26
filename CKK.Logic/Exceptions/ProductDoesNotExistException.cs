
namespace CKK.Logic.Exceptions
{
    class ProductDoesNotExistException : Exception
    {
        public ProductDoesNotExistException() : base() { }
        public ProductDoesNotExistException(string message) : base(message) { }
        public ProductDoesNotExistException(string message, Exception innerException) : base(message, innerException) { }
    }
}
