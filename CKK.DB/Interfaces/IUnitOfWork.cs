namespace CKK.DB.Interfaces
{
    public interface IUnitOfWork
    {
        IProductRepository Products { get; }
        IOrderRepository Orders { get; }
        IShoppingCartRepository ShoppingCarts { get; }
    }
}
