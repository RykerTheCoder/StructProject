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
    // class that acts as an access point to the Orders table
    public class OrderRepository : IOrderRepository
    {
        // get the connection to the database
        private readonly IConnectionFactory _connectionFactory;
        public OrderRepository(IConnectionFactory conn)
        {
            _connectionFactory = conn;
        }
        public int Add(Order entity) // method for adding a new order to the table
        {
            string sql = "INSERT INTO Orders (OrderId, OrderNumber, CustomerId, ShoppingCartId) VALUES (@OrderId, @OrderNumber, @CustomerId, @ShoppingCartId)";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }

        public int Delete(Order entity) // method for deleting an order from the table
        {
            string sql = "DELETE FROM Orders WHERE OrderId = @OrderId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }

        public List<Order> GetAll() // method to retrieve all orders from the table
        {
            string sql = "SELECT * FROM Orders";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Query<Order>(sql).ToList();
                return result;
            }
        }

        public Order GetById(int id) // method that returns the order with the given Id
        {
            string sql = "SELECT * FROM Orders WHERE OrderId = @OrderId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<Order>(sql, new { OrderId = id});
                return result;
            }
        }

        public Order GetOrderByCustomerId(int id) // method for getting a customer's order using the customer id
        {
            string sql = "SELECT * FROM Orders WHERE CustomerId = @CustomerId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<Order>(sql);
                return result;
            }
        }

        public int Update(Order entity) // update an order in the database
        {
            string sql = "UPDATE Orders SET OrderId = @OrderId, OrderNumber = @OrderNumber, CustomerId = @CustomerId, ShoppingCartId = @ShoppingCartId WHERE OrderId = @OrderId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }
    }
}
