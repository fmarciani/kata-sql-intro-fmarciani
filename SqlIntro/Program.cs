using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace SqlIntro
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
#if DEBUG
                .AddJsonFile("appsettings.Debug.json")
#else
                .AddJsonFile($"appsettings.Release.jason")
#endif
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            var repo = new DapperProductRepo(connectionString);
            var insProd = new Product { Name = "Bob's Burger" }; 
            var delProd = new Product() { Id = 999 }; 
            var upProd = new Product() { Name = "Bob's Burger", Id = 5000 };

            // Get product
            foreach (var prod in repo.GetProducts())
            {
                Console.WriteLine("Product Name: " + prod.Name);
            }

            // Delete product
            repo.DeleteProduct(delProd.Id);

            // Update product
            repo.UpdateProduct(upProd);

            // Insert product
            repo.InsertProduct(insProd);

            foreach (var prod in repo.GetProductsWithReview())
            {
                Console.WriteLine("\nProduct Name: " + prod.Name + "\nProduct Review: " + prod.Comments);
            }

            foreach (var prod in repo.GetProductsAndReviews())
            {
                Console.WriteLine("\nProduct Name: " + prod.Name + "\nProduct Review (if available): " + prod.Comments);
            }

            Console.ReadLine();
        } 
    }
}
