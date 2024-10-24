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
            productService.GetSellerProducts(currentUser);
            int count = productService.GetSellerProductCount(currentUser.UserId);
            if (count > 0)
            {
                Console.WriteLine("Enter the Product ID to update:");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    productService.UpdateProduct(productId, currentUser);
                }
                else
                {
                    Console.WriteLine("Invalid Product ID. Please try again.");
                }
            }
        }

        public void UpdateProductQuantity(User currentUser)
        {
            productService.GetSellerProducts(currentUser);
            int count = productService.GetSellerProductCount(currentUser.UserId);
            if (count > 0)
            {
                Console.WriteLine("Enter the Product ID to update:");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    productService.AddProductQuantity(productId);
                }
                else
                {
                    Console.WriteLine("Invalid Product ID. Please try again.");
                }
            }
        }

        public void DeleteProduct(User currentUser)
        {
            productService.GetSellerProducts(currentUser);
            if (productService.GetSellerProductCount(currentUser.UserId) > 0)
            {
                Console.WriteLine("Enter the Product ID to Delete:");
                if (int.TryParse(Console.ReadLine(), out int productId))
                {
                    List<Order> orders = orderService.ShowSellerOrders(currentUser);
                    var orderWithProduct = orders.Find(o => o.ProductListToBeOrdered.Any(p => p.Id == productId));
                    if (orderWithProduct != null)
                    {
                        if (orderWithProduct.Status == OrderStatus.Delivered || orderWithProduct.Status == OrderStatus.Canceled)
                        {

                            if (productService.DeleteProduct(productId, currentUser))
                            {
                                Console.WriteLine("Product Deleted Succcessfully !! ");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Product Id..");

                            }
                        }
                        else
                        {
                            Console.WriteLine("Cannot delete product as it is part of an order that is not yet delivered or canceled.");
                        }
                    }
                    else
                    {
                        if (productService.DeleteProduct(productId, currentUser))
                        {
                            Console.WriteLine("Product deleted successfully!");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Product Id.");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid Product Id.");
                }

            }
            else
            {
                Console.WriteLine("No Products Available to delete");
            }
        }


    }
}
