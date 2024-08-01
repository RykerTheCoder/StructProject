using CKK.Logic.Interfaces;
using CKK.Logic.Exceptions;
namespace CKK.Logic.Models
{
    // class that represents an item that the customer has in the shopping cart
    public class ShoppingCartItem : InventoryItem
    {
        public Product Product { get; set; }
        public int ShoppingCartId { get; set; }
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        private int _quantity;
        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if(value >= 0) //make sure the quantity doesnt become negative
                {
                    _quantity = value;
                }
                else
                {
                    throw new InventoryItemStockTooLowException();
                }
            }
        }
        public decimal GetTotal()
        {
            return Product.Price * Quantity;
        }
    }
}
