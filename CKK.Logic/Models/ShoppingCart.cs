
namespace CKK.Logic.Models
{
    public class ShoppingCart
    {
        private Customer _customer;
        private List<ShoppingCartItem> _products = new List<ShoppingCartItem>();

        public ShoppingCart(Customer cust)
        {
            _customer = cust;
        }

        public int GetCustomerId()
        {
            return _customer.GetId();
        }

        public ShoppingCartItem GetProductById(int id)
        {
            var prod =
                from item in _products
                let prodId = item.GetProduct().GetId()
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
            if (quantity < 0)
            {
                return null;
            }
            else
            {
                var item =
                from element in _products
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
                    _products.Add(newProduct);
                    return newProduct;
                }
            }
        }

        public ShoppingCartItem RemoveProduct(Product prod, int quantity)
        {

            if (quantity < 0)
            {
                return null;
            }
            else
            {
                var item =
                from element in _products
                let product = element.GetProduct()
                where product == prod
                select element;

                var result = item;

                if (result.Any())
                {
                    ShoppingCartItem theItem = item.First();
                    

                    if (theItem.GetQuantity() > quantity)
                    {
                        theItem.SetQuantity(theItem.GetQuantity() - quantity);
                        return theItem;
                    }
                    else
                    {
                        _products.Remove(theItem);
                        return null;
                    }
                }
                else
                {
                    ShoppingCartItem newProduct = new ShoppingCartItem(prod, quantity);
                    _products.Add(newProduct);
                    return newProduct;
                }
            }
        }

        public decimal GetTotal()
        {
            decimal total = 0;

            foreach (ShoppingCartItem item in _products)
            {
                total += item.GetTotal();
            }
            return total;
        }

        public List<ShoppingCartItem> GetProducts()
        {
            return _products;
        }
    }
}
