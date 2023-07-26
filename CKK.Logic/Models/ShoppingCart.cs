using CKK.Logic.Interfaces;
using CKK.Logic.Exceptions;

namespace CKK.Logic.Models
{
    public class ShoppingCart : IShoppingCart
    {
        public List<ShoppingCartItem> Products { get; set; } = new List<ShoppingCartItem>();
        public Customer Customer { get; set; }

        public ShoppingCart(Customer cust)
        {
            Customer = cust;
        }

        public int GetCustomerId()
        {
            return Customer.Id;
        }

        public ShoppingCartItem GetProductById(int id)
        {
            if (id < 0)
            {
                throw new InvalidIdException("Id is less than 0.");
            }

            var prod =
                from item in Products
                let prodId = item.GetProduct().Id
                where prodId == id
                select item;
            var result = prod;

            if (result.Any())
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }

        public ShoppingCartItem AddProduct(Product prod, int quantity)
        {
            if (quantity <= 0)
            {
                throw new InventoryItemStockTooLowException("Quantity added is less than or equal to 0.");
            }
            else
            {
                var item =
                    from element in Products
                    let product = element.GetProduct()
                    where product == prod
                    select element;

                var result = item;

                if (result.Any())
                {
                    ShoppingCartItem theItem = item.First();
                    theItem.SetQuantity(theItem.GetQuantity() + quantity);
                    return theItem;
                }
                else
                {
                    ShoppingCartItem newProduct = new ShoppingCartItem(prod, quantity);
                    Products.Add(newProduct);
                    return newProduct;
                }
            }
        }

        public ShoppingCartItem RemoveProduct(int id, int quantity)
        {
            if (quantity < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(quantity), quantity, "Quantity removed is less than 0.");
            }

            ShoppingCartItem prod = GetProductById(id);

            if (prod == null)
            {
                throw new ProductDoesNotExistException("Product being removed does not exist.");
            }

            if (prod.GetQuantity() - quantity <= 0)
            {
                prod.SetQuantity(0);
                Products.Remove(prod);
            }
            else
            {
                prod.SetQuantity(prod.GetQuantity() - quantity);
            }
            return prod;

        }

        public decimal GetTotal()
        {
            decimal total = 0;

            foreach (ShoppingCartItem item in Products)
            {
                total += item.GetTotal();
            }
            return total;
        }

        public List<ShoppingCartItem> GetProducts()
        {
            return Products;
        }
    }
}
