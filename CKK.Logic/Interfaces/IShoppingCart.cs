
using CKK.Logic.Models;

namespace CKK.Logic.Interfaces
{
    public interface IShoppingCart
    {
        int CustomerId { get; set; }
        ShoppingCartItem AddProduct(Product prod, int quantity);
        ShoppingCartItem RemoveProduct(int id, int quantity);
        decimal GetTotal();
        ShoppingCartItem GetProductById(int id);
        List<ShoppingCartItem> GetProducts();
    }
}
