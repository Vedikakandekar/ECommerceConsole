using ECommerce.Repositories;
using ECommerceClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceClassLibrary.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private static List<Product> _products;
        static ProductRepository()
        {

            _products = new List<Product>
        {
            new Product ( 1, "Laptop", "A high-end laptop", 1200.00m,  10,101),
            new Product ( 2, "Smartphone","Latest smartphone",  800.00m,  50,101 ),
            new Product ( 3, "Headphones",  "Noise-cancelling headphones", 150.00m, 30,101)

        };
        }

        public void AddProduct(Product newProduct)
        {

            _products.Add(newProduct);
            Console.WriteLine($"ID: {newProduct.Id}, Name: {newProduct.Name}, Price: {newProduct.Price:C}, Quantity: {newProduct.Quantity}");
            Console.WriteLine("Product added successfully!");
        }

        public bool DeleteProduct(Product product)
        {
           
            if (product != null)
            {
                _products.Remove(product);
                return true;
            }
            return false;
        }

        public void ShowAllProducts()
        {
            Console.WriteLine("===== Product List =====");

            if (_products.Count == 0)
            {
                Console.WriteLine("No products available.");
            }
            else
            {
                foreach (var product in _products)
                {
                    Console.WriteLine($"ID: {product.Id} ");
                    Console.Write($"Name: {product.Name} ");
                    Console.Write($"Description: {product.Description} ");
                    Console.WriteLine($"Price: {product.Price} ");
                    Console.WriteLine("---------------------------------------------------------");
                }
            }

        }

        public List<Product> ShowSellerProducts(User seller)
        {
           

           return  _products.Where(p => p.SellerId == seller.UserId).ToList();


            

        }


        public Product GetProductById(int id)
        {
           
            Product prod = _products.FirstOrDefault(p => p.Id == id);
          
            return prod;
        }

        public void UpdateProduct(Product updatedProduct )
        {
            var product = _products.FirstOrDefault(p => p.Id == updatedProduct.Id);
            if (product != null)
            {
                product.Name = updatedProduct.Name;
                product.Description = updatedProduct.Description;
                product.Price = updatedProduct.Price;
                product.Quantity = updatedProduct.Quantity;
            }
        }

        public void AddProductQuantity(Product productToUpdate, decimal additionalQuantity)
        {
            var product = _products.FirstOrDefault(p => p.Id == productToUpdate.Id);
            product.Quantity += additionalQuantity;
        }

        public int GetSellerProductCount(int sellerId)
        {
            return _products.Count(p => p.SellerId == sellerId);
        }

        public int GetAllProductCount()
        {
            return _products.Count();
        }
    }
}

