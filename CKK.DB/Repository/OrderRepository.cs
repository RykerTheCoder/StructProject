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
    public class OrderRepository : IOrderRepository
    {
        private readonly IConnectionFactory _connectionFactory;
        public OrderRepository(IConnectionFactory conn)
        {
            _connectionFactory = conn;
        }
        public int Add(Order entity)
        {
            string sql = "INSERT INTO Orders (OrderNumber, CustomerId, ShoppingCartId) VALUES (OrderNumber = @OrderNumber, CustomerId = @CustomerId, ShoppingCartId = @ShoppingCartId)";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }

        public int Delete(Order entity)
        {
            string sql = "DELETE FROM Orders WHERE OrderId = @OrderId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }

        public List<Order> GetAll()
        {
            string sql = "SELECT * FROM Orders";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Query<Order>(sql).ToList();
                return result;
            }
        }

        public Order GetById(int id)
        {
            string sql = "SELECT * FROM Orders WHERE OrderId = @OrderId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<Order>(sql, new { OrderId = id});
                return result;
            }
        }

        public Order GetOrderByCustomerId(int id)
        {
            string sql = "SELECT * FROM Orders WHERE CustomerId = @CustomerId";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<Order>(sql);
                return result;
            }
        }

        public int Update(Order entity)
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
