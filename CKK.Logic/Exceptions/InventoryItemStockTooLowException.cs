
namespace CKK.Logic.Exceptions
{
    class InventoryItemStockTooLowException : Exception
    {
        public InventoryItemStockTooLowException() : base() { }
        public InventoryItemStockTooLowException(string message) : base(message) { }
        public InventoryItemStockTooLowException(string message, Exception innerException) : base(message, innerException) { }
    }
}
