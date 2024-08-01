using CKK.Logic.Models;

namespace CKK.DB.Interfaces
{
    public interface IShoppingCartRepository
    {
        ShoppingCartItem AddToCart(int ShoppingCartId, int ProductId, int quantity);
        int ClearCart(int shoppingCartId);
        decimal GetTotal(int shoppingCartId);
        List<ShoppingCartItem> GetProducts(int shoppingCartId);
        void Ordered(int shoppingCartId);
        int Update(ShoppingCartItem entity);
        int Add(ShoppingCartItem entity);
    }
}
