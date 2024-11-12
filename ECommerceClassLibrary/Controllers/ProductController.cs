using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories;
using ECommerceClassLibrary.Repositories.Contracts;
using ECommerceClassLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceClassLibrary.Controllers
{
    public class ProductController
    {
        private readonly IProductService productService;
        private readonly IOrderService orderService;

        public ProductController(IProductService productService, IOrderService orderService)
        {
            this.productService = productService;
            this.orderService = orderService;
           
        }
        public void ShowSellerProducts(User currentUser)
        {
            productService.GetSellerProducts(currentUser);
        }

        public void AddProduct(User currentUser)
        {

            productService.AddProduct(currentUser);
        }

        public void ShowAllProducts()
        {
            productService.ShowAllProducts();
        }

        public void UpdateProduct(User currentUser)
        {
            if (!HasSellerProducts(currentUser))
            {
                return;
            }
            int productId = GetProductIdFromUser("Enter the Product ID to update:");
            if (productId != -1)
            {
                productService.UpdateProduct(productId, currentUser);
            }
        }

        public void UpdateProductQuantity(User currentUser)
        {
            if (!HasSellerProducts(currentUser))
            {
                return;
            }
            int productId = GetProductIdFromUser("Enter the Product ID to update quantity:");
            if (productId != -1)
            {
                productService.AddProductQuantity(productId);
            }
        }

        public void DeleteProduct(User currentUser)
        {
            if (!HasSellerProducts(currentUser))
            {
                return;
            }
            int productId = GetProductIdFromUser("Enter the Product ID to delete:");

            if (productId <= -1)
            {
                return;
            }
            if (!CanDeleteProduct(currentUser, productId))
            {
                return;
            }

                    if (productService.DeleteProduct(productId, currentUser))
                    {
                        Console.WriteLine("Product deleted successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid Product ID.");
                    }
        }

        private bool HasSellerProducts(User currentUser)
        {
            productService.GetSellerProducts(currentUser);
            int count = productService.GetSellerProductCount(currentUser.UserId);
            if (count == 0)
            {
                Console.WriteLine("No products available.");
                return false;
            }
            return true;
        }

        private int GetProductIdFromUser(string promptMessage)
        {
            Console.WriteLine(promptMessage);
            if (int.TryParse(Console.ReadLine(), out int productId))
            {
                return productId;
            }
            else
            {
                Console.WriteLine("Invalid Product ID. Please try again.");
                return -1;
            }
        }

        private bool CanDeleteProduct(User currentUser, int productId)
        {
            List<Order> orders = orderService.ShowSellerOrders(currentUser);
            var orderWithProduct = orders.Find(o => o.ProductListToBeOrdered.Any(p => p.Id == productId));

            if (orderWithProduct == null)
            {
                return true;
            }
                
                if (orderWithProduct.Status == OrderStatus.Delivered || orderWithProduct.Status == OrderStatus.Canceled)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Cannot delete product as it is part of an order that is not yet delivered or canceled.");
                    return false;
                }
           
        }
    }

}
