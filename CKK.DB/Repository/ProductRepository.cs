using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKK.DB.Interfaces;
using CKK.Logic.Models;
using Dapper;

namespace CKK.DB.Repository
{
    // class that acts as an access point to the Products table
    public class ProductRepository : IProductRepository
    {
        // get the connection to the database
        private readonly IConnectionFactory _connectionFactory;
        public ProductRepository(IConnectionFactory conn)
        {
            _connectionFactory = conn;
        }
        public int Add(Product entity) // method for adding a product to the table
        {
            string sql = "INSERT INTO Products (Price,Quantity,Name) VALUES (@Price,@Quantity,@Name)";
            using (IDbConnection connection  = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }

        public int Delete(Product entity) // method for deleting a product from the table
        {
            string sql = "DELETE FROM Products WHERE Id = @Id";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }

        public List<Product> GetAll() // method for retrieving all products from the table
        {
            string sql = "SELECT * FROM Products";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Query<Product>(sql).ToList();
                return result;
            }
        }

        public Product GetById(int id) // method for retrieving all products that have the given Id
        {
            string sql = "SELECT * FROM Products WHERE Id = @Id";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.QuerySingleOrDefault<Product>(sql, new { Id = id });
                return result;
            }
        }

        public List<Product> GetByName(string name) // method for retrieving all products that have the specified string inside of the name
        {
            string sql = "SELECT * FROM Products WHERE Name LIKE CONCAT('%', @Name, '%')";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Query<Product>(sql, new { Name = name }).ToList();
                return result;
            }
        }

        public int Update(Product entity) // method for updating a product in the database
        {
            string sql = "UPDATE Products SET Price = @Price, Quantity = @Quantity, Name = @Name WHERE Id = @Id";
            using (IDbConnection connection = _connectionFactory.GetConnection)
            {
                connection.Open();
                var result = connection.Execute(sql, entity);
                return result;
            }
        }
    }
}
