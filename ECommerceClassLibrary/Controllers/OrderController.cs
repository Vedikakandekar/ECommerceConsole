using ECommerceClassLibrary.Helper;
using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories;
using ECommerceClassLibrary.Repositories.Contracts;
using ECommerceClassLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceClassLibrary.Controllers
{
    public class OrderController
    {

        private readonly IOrderService orderService;
        private readonly IProductService productService;
        private readonly IUserService userService;

        public OrderController(IOrderService orderService, IProductService productService, IUserService userService)
        {
            this.orderService = orderService;
            this.productService = productService;
            this.userService = userService;
            orderService.OrderProcessed += Ashwin;
        }

        private void Ashwin(object sender, OrderEventArgs e)
        {
            Customer user = (Customer)userService.GetUserById(e.CustomerId);
            user.notifications.Add($"\n\nNotification: Order ID {e.OrderId} is  now {e.Status}.\n\n");
        }


        public void ShowAllOrders()
        {
            List<Order> AllOrders = orderService.ShowAllOrders();
            foreach (var order in AllOrders)
            {
                System.Console.WriteLine(order);
            }
        }

        public void ShowCustomerOrders(User currentUser)
        {
            orderService.GetCustomerOrders(currentUser);
        }
        public void ShowOrdersForSeller(User currentUser)
        {
             Console.WriteLine("===== Your Orders =====");
            List<Order> orders = orderService.ShowSellerOrders(currentUser);

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders have been placed for your products.");
                return;
            }

            DisplayOrders(orders);

        }


        public void DisplayOrders(List<Order> orders)
        {
            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer ID: {order.CustomerId}, Total Amount: {order.TotalAmount}, Order Date: {order.OrderDate}, Order Status : {order.Status}");
                Console.WriteLine("Ordered Product:");
                foreach (var product in order.ProductListToBeOrdered)
                {
                    Console.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}");
                }
                Console.WriteLine("------------------------------------------------------------");
            }
        }

        public void UpdateOrderStatus(User currentUser)
        {
            Console.WriteLine("===== Update Order Status =====");

            List<Order> OrdersForSeller;

            OrdersForSeller = orderService.ShowSellerOrders(currentUser);
            if (OrdersForSeller.Count == 0)
            {
                Console.WriteLine("No orders available to update.");
                return;
            }
            DisplayOrders(OrdersForSeller);
            Console.Write("\n");

            Console.WriteLine("Enter the Order ID you want to update: ");

            if (int.TryParse(Console.ReadLine(), out int orderId))
            {

                Order orderToUpdate = orderService.GetOrderById(orderId);

                if (orderToUpdate == null)
                {
                    System.Console.WriteLine("Order Not Found !!");
                }
                else
                {

                    Console.WriteLine("\nCurrent Status  : " + orderToUpdate.Status);

                    Console.WriteLine("\nAvailable Order Statuses:");

                    foreach (var status in Enum.GetValues(typeof(OrderStatus)))
                    {
                        Console.WriteLine($"{(int)status} - {status}");
                    }
                    Console.Write("Enter the status number to update the order: ");

                    if (int.TryParse(Console.ReadLine(), out int statusChoice))
                    {


                        orderService.UpdateOrderStatus(orderToUpdate, statusChoice);
                        Console.WriteLine("Order Status is Updated !!! ");

                    }


                }
            }
            else
            {
                Console.WriteLine("Invalid Input.. ");

            }

        }


        public async Task PlaceOrder(User currentUser)
        {
            Console.WriteLine("===== Place Order =====");


            Product selectedProduct;
            decimal quantity;


            while (true)
            {
                Console.Write("\nEnter the Product ID you want to order: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int productId) || productId <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric Product ID.");
                    continue;
                }


                selectedProduct = productService.GetProductById(productId);


                if (selectedProduct == null)
                {
                    Console.WriteLine("Invalid Product ID. Please try again.");
                    continue;
                }
                else
                {
                    Console.WriteLine($"\nSelected Product: {selectedProduct.Name}, Price: {selectedProduct.Price:C}");


                    (decimal quantity1,bool b1) = ValidationHelper.GetValidatedDecimalInput("\nEnter the quantity you want to order: ");
                    quantity = quantity1;
                    if(!b1)
                    {
                        Console.WriteLine("Order Canceled..Try again..!!");
                        return;
                    }
                    else if (quantity > selectedProduct.Quantity)
                    {
                        Console.WriteLine($"\nInsufficient stock. Only {selectedProduct.Quantity} units available. Please try again.");
                        return;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            try
            {


                Console.WriteLine($"\n\n===== Order Summary =====");
                Console.WriteLine($"Product: {selectedProduct.Name}");
                Console.WriteLine($"Quantity: {quantity}");
                Console.WriteLine($"Total Price: {selectedProduct.Price * quantity:C}");

                Console.Write("Do you want to place the order? (yes : if you want to place): ");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "yes")
                {
                    selectedProduct.Quantity -= quantity;
                    var orderedProduct = new List<Product>
                 {
            new Product(selectedProduct.Id,selectedProduct.Name,selectedProduct.Description,
                                     selectedProduct.Price,quantity,selectedProduct.SellerId)
                  };
                    await orderService.PlaceOrder(orderedProduct, currentUser);
                   
                }
                else
                {
                    Console.WriteLine("Order canceled.");
                }
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);

                Console.ReadKey();
            }

        }

    }
}
