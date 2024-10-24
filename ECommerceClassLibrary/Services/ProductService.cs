using ECommerce.Repositories;
using ECommerceClassLibrary.Helper;
using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories.Contracts;
using ECommerceClassLibrary.Services;
using System;
using System.Collections.Generic;

namespace ECommerceClassLibrary.Repositories
{
    public class ProductService : IProductService
    {
        IProductRepository repository;

        public ProductService(IProductRepository repository)
        {
            this.repository = repository;
        }

        public Product GetProductDetailsFromUser(User currentUser)
        {

            (string name,bool b1) = ValidationHelper.GetValidUserName("Enter Product Name: "); ;
            if(!b1)
                return null;

            (string description,bool b2) = ValidationHelper.GetValidatedStringInput("Enter Product Description: ");
             if(!b2)
                return null;

            (decimal price,bool b3) = ValidationHelper.GetValidatedDecimalInput("Enter Product Price: ");
            if(!b3)
                return null;
            (decimal quantity,bool b4) = ValidationHelper.GetValidatedDecimalInput("Enter Product Quantity: ");
            if(!b4)
                return null;

            int productCount = repository.GetAllProductCount();

            int newProductId = productCount > 0 ? productCount + 1 : 1;
            return new Product
            (
                 newProductId,
                 name,
                 description,
                 price,
                 quantity,
                 currentUser.UserId
            );
        }
        public void AddProduct(User currentUser)
        {
            Console.WriteLine("===== Add New Product =====");
            Product newProduct = GetProductDetailsFromUser(currentUser);
            if(newProduct==null)
            {
                System.Console.WriteLine("Product Not added..");
                return;
            }
            
            repository.AddProduct(newProduct);
        }

        public void DisplayProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            return repository.GetProductById(id);
        }

        public void ShowAllProducts()
        {
            repository.ShowAllProducts();
        }

        public int GetSellerProductCount(int userId)
        {
           return repository.GetSellerProductCount(userId);
        }

        public List<Product> GetSellerProducts(User currentUser)
        {
            Console.WriteLine($"===== {currentUser.Name}'s Products =====");
            var sellerProducts = repository.ShowSellerProducts(currentUser);

            if (sellerProducts.Count == 0)
            {
                Console.WriteLine("No products available.");
            }
            else
            {
                foreach (var product in sellerProducts)
                {
                    Console.WriteLine($"ID: {product.Id} ");
                    Console.Write($"Name: {product.Name}  ");
                    Console.Write($"Description: {product.Description}  ");
                    Console.Write($"Price: {product.Price}  ");
                    Console.WriteLine($"Quantity: {product.Quantity}  ");
                    Console.WriteLine("---------------------------------------");
                }

            }
            return sellerProducts;
        }

        public Product GetUpdatedProduct(Product product)
        {
            Console.WriteLine($"===== Update Product: {product.Name} =====");
            Console.WriteLine($"ID: {product.Id}");
            Console.WriteLine($"Current Name: {product.Name}");
            Console.WriteLine($"Current Description: {product.Description}");
            Console.WriteLine($"Current Price: {product.Price}");
            Console.WriteLine($"Current Quantity: {product.Quantity}");
            Console.WriteLine("------------------------------");

            Console.WriteLine("Enter new name (leave blank to keep current):");
            string newName = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newName))
            {
                product.Name = newName;
            }

            Console.WriteLine("Enter new description (leave blank to keep current):");
            string newDescription = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(newDescription))
            {
                product.Description = newDescription;
            }

            Console.WriteLine("Enter new price (leave blank to keep current):");
            string newPriceInput = Console.ReadLine();
            if (decimal.TryParse(newPriceInput, out decimal newPrice))
            {
                product.Price = newPrice;
            }

            Console.WriteLine("Enter new Quantity (leave blank to keep current):");
            string newQuantInput = Console.ReadLine();
            if (decimal.TryParse(newQuantInput, out decimal newQuant))
            {
                product.Quantity = newQuant;
            }
            return product;
        }

        public bool UpdateProduct(int productId, User currentUser)
        {


            var product = repository.GetProductById(productId);
            if (product == null || product.SellerId!=currentUser.UserId)
            {
                Console.WriteLine("Product not found.");
                return false;
            }

            Product updatedProduct = GetUpdatedProduct(product);
           
            repository.UpdateProduct(updatedProduct);
            Console.WriteLine("Product updated successfully.");
            return true;
        }

        public void AddProductQuantity(int productId)
        {
            Product product = repository.GetProductById(productId);

            if (product == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }

            Console.WriteLine($"Current Quantity: {product.Quantity}");
            Console.WriteLine("Enter  quantity to add:");
            string newQuantityInput = Console.ReadLine();
            if (decimal.TryParse(newQuantityInput, out decimal newQuantity))
            {
                if (newQuantity <= 0)
                {
                    Console.WriteLine("Enter Valid Quantity ...It cannot be Zero or Negative.");
                }
                else
                {
                    repository.AddProductQuantity(product, newQuantity);
                    Console.WriteLine("Product updated successfully.");
                }
            }
            else
            {
                System.Console.WriteLine("Enter Valid Quantity !!!");
            }


        }

        public bool DeleteProduct(int productId,User currentUser)
        {
            var product = GetProductById(productId);
            if (product != null && product.SellerId==currentUser.UserId)
            {
            

                return repository.DeleteProduct(product);
            }
            else
            {
                System.Console.WriteLine("Product does not exist !!!");
                return false;
            }
        }
    }
}
