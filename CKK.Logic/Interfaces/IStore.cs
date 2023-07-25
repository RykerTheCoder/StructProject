
using CKK.Logic.Models;

namespace CKK.Logic.Interfaces
{
    public interface IStore
    {
        StoreItem AddStoreItem(Product prod, int quantity);
        StoreItem RemoveStoreItem(int id, int quantity);
        StoreItem FindStoreItemById(int id);
        List<StoreItem> GetStoreItems();
    }
}
