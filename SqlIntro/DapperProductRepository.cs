using System;
using System.Collections.Generic;
using System.IO;
using Dapper;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace SqlIntro
{
    class DapperProductRepo : IProductRepository
    {
        private readonly string _connectionString;

        public DapperProductRepo(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<Product> GetProducts()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                return conn.Query<Product>("SELECT ProductId as Id, Name from product");
            }
        }

        public void DeleteProduct(int id)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute("DELETE from Product WHERE ProductID = @id", new { id });
            }
        }

        public void UpdateProduct(Product prod)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute("UPDATE product SET Name = @name WHERE ProductID = @id", new { prod.Name, prod.Id });
            }
        }

        public void InsertProduct(Product prod)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                conn.Execute("INSERT INTO product (name) VALUES(@name)", new { prod.Name });
            }
        }

        public IEnumerable<Product> GetProductsWithReview()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                //conn.Execute("SELECT p.Name, pr.Comments FROM product AS p INNER JOIN productreview AS pr ON p.ProductId = pr.ProductId");
                return conn.Query<Product>("SELECT p.ProductId as Id, p.Name, pr.Comments FROM product AS p INNER JOIN productreview AS pr ON p.ProductId = pr.ProductId");

            }
        }

        public IEnumerable<Product> GetProductsAndReviews()
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                //conn.Execute("SELECT p.ProductId, p.Name, pr.Comments FROM product AS p LEFT OUTER JOIN productreview AS pr ON p.ProductId = pr.ProductId");
                return conn.Query<Product>("SELECT p.ProductId, p.Name, pr.Comments FROM product AS p LEFT OUTER JOIN productreview AS pr ON p.ProductId = pr.ProductId");
            }
        }
    }
}