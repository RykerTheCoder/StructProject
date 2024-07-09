
using CKK.Logic.Models;

namespace CKK.Logic.Interfaces
{
    public interface IStore
    {
        StoreItem AddStoreItem(Product prod, int quantity);
        StoreItem RemoveStoreItem(int id, int quantity);
        StoreItem FindStoreItemById(int id);
        List<StoreItem> GetStoreItems();
        void DeleteStoreItem(int id);
        List<StoreItem> GetAllProductsByName(string name);
        List<StoreItem> GetProductsByQuantity();
        List<StoreItem> GetProductsByPrice();
    }
}
