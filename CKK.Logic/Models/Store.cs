
namespace CKK.Logic.Models
{
    public class Store
    {
        private int _id;
        private string _name;
        private List<StoreItem> items = new List<StoreItem>();

        public int GetId()
        {
            return _id;
        }
        public void SetId(int id)
        {
            _id = id;
        }
        public string GetName()
        {
            return _name;
        }
        public void SetName(string name)
        {
            _name = name;
        }
        public StoreItem AddStoreItem(Product prod, int quantity)
        {
            if (quantity > 0)
            {
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
            return null;
        }
        public StoreItem RemoveStoreItem(int id, int quantity)
        {
            StoreItem item = FindStoreItemById(id);

            if (item != null)
            {
                if (item.GetQuantity() >= quantity)
                {
                    item.SetQuantity(item.GetQuantity() - quantity);
                }
                else
                {
                    item.SetQuantity(0);
                }
                return item;
            }
            return null;
        }
        public List<StoreItem> GetStoreItems()
        {
            return items;
        }
        public StoreItem FindStoreItemById(int id)
        {
            var storeItem =
                from item in items
                let prodId = item.GetProduct().GetId()
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
