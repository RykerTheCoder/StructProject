using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKK.DB.Interfaces;
using CKK.Logic.Interfaces;
using CKK.Logic.Models;
using Dapper;

namespace CKK.DB.Repository
{
    // class that acts as an access point to the ShoppingCartItems table
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        // get the connection to the database
        private readonly IConnectionFactory _connectionFactory;
        public ShoppingCartRepository(IConnectionFactory conn)
        {
            _connectionFactory = conn;
        }
        public int Add(ShoppingCartItem entity) // Add a new item to the ShoppingCartItems table
        {
            string sql = "INSERT INTO ShoppingCartItems (ShoppingCartId,ProductId,Quantity) VALUES (@ShoppingCartId, @ProductId, @Quantity)";
            using(IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }

        public ShoppingCartItem AddToCart(int ShoppingCartId, int ProductId, int quantity) // same thing as the Add method except it has validation and different inputs
        {
            using(IDbConnection connection = _connectionFactory.GetConnection)
            {
                ProductRepository _productRepository = new ProductRepository(_connectionFactory);
                Product item = _productRepository.GetById(ProductId);
                var productItems = GetProducts(ShoppingCartId).Find(x => x.ProductId == ProductId);

                ShoppingCartItem shopItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    ProductId = ProductId,
                    Quantity = quantity,
                };

                if (item.Quantity >= quantity)
                {
                    if (productItems != null)
                    {
                        // product is already in cart, so update quantity
                        Update(shopItem);
                    }
                    else
                    {
                        // product doesnt exist yet in the cart, so add it
                        Add(shopItem);
                    }
                }
                return shopItem;
            }
        }

        public int ClearCart(int shoppingCartId) // Removes the item from the table
        {
            string sql = "DELETE FROM ShoppingCartItems WHERE ShoppingCartId = @ShoppingCartId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, new {ShoppingCartId = shoppingCartId});
                return result;
            }
        }

        public List<ShoppingCartItem> GetProducts(int shoppingCartId) // get all items with the provided shoppingCartId
        {
            string sql = "SELECT * FROM ShoppingCartItems WHERE ShoppingCartId = @ShoppingCartId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Query<ShoppingCartItem>(sql, new { ShoppingCartId = shoppingCartId }).ToList();
                return result;
            }
        }

        public decimal GetTotal(int shoppingCartId) // Gets the total price of all items (the order) with the provided shopping cart id
        {
            string sql = "SELECT SUM(items.Quantity * Price) FROM ShoppingCartItems items, Products prods WHERE items.ProductId = prods.Id AND ShoppingCartId = @ShoppingCartId";
            decimal result;
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                try
                {
                    result = connection.QuerySingleOrDefault<decimal>(sql, new { ShoppingCartId = shoppingCartId });
                }
                catch
                {
                    result = 0;
                }
            
            }
            return result;
        }
        public void Ordered(int shoppingCartId) // exact same thing as ClearCart except it doesnt return anything
        {
            string sql = "DELETE FROM ShoppingCartItems WHERE ShoppingCartId = @ShoppingCartId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                connection.Execute(sql, new { ShoppingCartId = shoppingCartId });
            }
        }

        public int Update(ShoppingCartItem entity) // Method for updating a row in the table
        {
            string sql = "UPDATE ShoppingCartItems SET ShoppingCartId = @ShoppingCartId, ProductId = @ProductId, Quantity = @Quantity WHERE ProductId = @ProductId AND ShoppingCartId = @ShoppingCartId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }
    }
}
