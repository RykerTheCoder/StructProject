using CKK.Logic.Interfaces;
using CKK.Logic.Exceptions;

namespace CKK.Logic.Models
{
    public class Store : Entity, IStore
    {
        private List<StoreItem> items = new List<StoreItem>();

        public StoreItem AddStoreItem(Product prod, int quantity)
        {
            if (quantity <= 0)
            {
                throw new InventoryItemStockTooLowException("Quantity added is less than or equal to 0.");
            }
            //Get the product that already exists in items that is equal to prod (if any)
            var itemThatAlreadyExists =
                from item in items
                where item.GetProduct() == prod
                select item;

            // this statement makes it so I have to query just once
            var result = itemThatAlreadyExists;

            // Check if there are any items that equal prod
            if (result.Any())
            {
                StoreItem product = result.First();
                product.SetQuantity(product.GetQuantity() + quantity);
                return product;
            }
            else
            {
                StoreItem product = new StoreItem(prod, quantity);
                items.Add(product);
                return product;
            }
        }
        public StoreItem RemoveStoreItem(int id, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Quantity removed is less than 0.");
            }

            StoreItem item = FindStoreItemById(id);

            if (item == null)
            {
                throw new ProductDoesNotExistException("Product being removed does not exist.");
            }

            if (item.GetQuantity() - quantity <= 0)
            {
                item.SetQuantity(0);
            }
            else
            {
                item.SetQuantity(item.GetQuantity() - quantity);
            }
            return item;
        }
        public List<StoreItem> GetStoreItems()
        {
            return items;
        }
        public StoreItem FindStoreItemById(int id)
        {
            if (id < 0)
            {
                throw new InvalidIdException("Id is less than 0.");
            }
            var storeItem =
                from item in items
                let prodId = item.GetProduct().Id
                where prodId == id
                select item;
            var result = storeItem;

            if (result.Any())
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }
    }
}
