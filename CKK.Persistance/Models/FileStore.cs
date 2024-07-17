/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using CKK.Logic.Exceptions;
using CKK.Logic.Interfaces;
using CKK.Logic.Models;
using CKK.Persistance.Interfaces;

namespace CKK.Persistance.Models
{
    public class FileStore : IStore, ISavable, ILoadable
    {
        private List<StoreItem> Items = new List<StoreItem>();
        public readonly string FilePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "Persistance" + Path.DirectorySeparatorChar + "StoreItems.dat";
        private int IdCounter;

        public FileStore()
        {
            CreatePath();
            Load();
        }
        private void CreatePath()
        {
            Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "Persistance");
        }
        public void Save()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Write))
            {
                formatter.Serialize(stream, Items);
            }
        }
        public void Load()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            if (File.Exists(FilePath))
            {
                using (var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read))
                {
                    if (stream.Length > 0)
                    {
                        Items = (List<StoreItem>)formatter.Deserialize(stream);
                    }
                }
            }
            else
            {
                File.Create(FilePath);
            }
        }


        public StoreItem AddStoreItem(Product prod, int quantity)
        {
            if (quantity < 0)
            {
                throw new InventoryItemStockTooLowException("Quantity added is less than 0.");
            }
            //Get the product that already exists in Items that is equal to prod (if any)
            var itemThatAlreadyExists =
                from item in Items
                where item.Product == prod
                select item;

            // this statement makes it so I have to query just once each method call
            var result = itemThatAlreadyExists;

            // Check if there are any Items that equal prod
            if (result.Any())
            {
                StoreItem product = result.First();
                product.Quantity += quantity;
                Save();
                return product;
            }
            else
            {
                if (prod.Id == 0)
                {
                    prod.Id = Items.Count + 1;
                }
                StoreItem product = new StoreItem(prod, quantity);
                Items.Add(product);
                Save();
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

            if (item.Quantity - quantity <= 0)
            {
                item.Quantity = 0;
            }
            else
            {
                item.Quantity -= quantity;
            }
            Save();
            return item;
        }
        public StoreItem FindStoreItemById(int id)
        {
            if (id < 0)
            {
                throw new InvalidIdException("Id is less than 0.");
            }
            var storeItem =
                from item in Items
                let prodId = item.Product.Id
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
        public List<StoreItem> GetStoreItems()
        {
            return Items;
        }
        public void DeleteStoreItem(int id)
        {
            var storeItem = FindStoreItemById(id);
            Items.Remove(storeItem);
            Save();
        }
        public List<StoreItem> GetAllProductsByName(string name)
        {
            List<StoreItem> result = new List<StoreItem>();
            foreach (var item in Items)
            {
                if (item.Product.Name.ToLower().StartsWith(name.ToLower()))
                {
                    result.Add(item);
                }
            }
            return result;
        }
        public List<StoreItem> GetProductsByQuantity()
        {
            return MergeSort(0, Items.Count - 1, "Quantity");
        }
        public List<StoreItem> GetProductsByPrice()
        {
            return MergeSort(0, Items.Count - 1, "Price");
        }
        private static List<StoreItem> Merge(List<StoreItem> list1, List<StoreItem> list2, string property)
        {
            int start1 = 0;
            int end1 = list1.Count - 1;
            int start2 = 0;
            int end2 = list2.Count - 1;
            
            //Create a version of the list that is sorted by its halfway point
            List<StoreItem> combined = new List<StoreItem>();
            if (property == "Price")
            {
                while (start1 <= end1 && start2 <= end2)
                {
                    if (list1[start1].Product.Price >= list2[start2].Product.Price)
                    {
                        combined.Add(list1[start1]);
                        start1++;
                    }
                    else
                    {
                        combined.Add(list2[start2]);
                        start2++;
                    }
                }
                // fill in the rest
                if (start1 <= end1)
                {
                    for (int i = start1; i <= end1; i++)
                    {
                        combined.Add(list1[i]);
                    }
                }
                else if (start2 <= end2)
                {
                    for (int i = start2; i <= end2; i++)
                    {
                        combined.Add(list2[i]);
                    }
                }
            }
            else if (property == "Quantity")
            {
                while (start1 <= end1 && start2 <= end2)
                {
                    if (list1[start1].Quantity >= list2[start2].Quantity)
                    {
                        combined.Add(list1[start1]);
                        start1++;
                    }
                    else
                    {
                        combined.Add(list2[start2]);
                        start2++;
                    }
                }
                // fill in the rest
                if (start1 <= end1)
                {
                    for (int i = start1; i <= end1; i++)
                    {
                        combined.Add(list1[i]);
                    }
                }
                else if (start2 <= end2)
                {
                    for (int i = start2; i <= end2; i++)
                    {
                        combined.Add(list2[i]);
                    }
                }
            }
            else
            {
                throw new ArgumentException("property is invalid");
            }
            return combined;
        }
        private List<StoreItem> MergeSort(int start, int end, string property)
        {
            List<StoreItem> result = new List<StoreItem>();
            // sort using the merge sort algorithm
            if (end - start + 1 > 1)
            {
                int middle1 = (start + end) / 2;
                int middle2 = middle1 + 1;

                result = Merge(MergeSort(start, middle1, property), MergeSort(middle2, end, property), property);
            }
            else
            {
                result.Add(Items[start]);
            }
            return result;
        }
    }
}
*/