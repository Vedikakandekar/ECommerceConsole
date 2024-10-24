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
            orderService.OrderProcessed += SendNotificationWhenOrderStatusChanged;
        }

        private void SendNotificationWhenOrderStatusChanged(object sender, OrderEventArgs e)
        {
            Customer user = (Customer)userService.GetUserById(e.CustomerId);
            user.notifications.Add($"\n\nNotification: Order ID {e.OrderId} is  now {e.Status}.\n\n");
        }
        public void ShowAllOrders()
        {
            System.Console.WriteLine("======    Your Orders   ======");
            List<Order> AllOrders = orderService.ShowAllOrders();
            foreach (var order in AllOrders)
            {
                System.Console.WriteLine(order);
            }
        }

        public void ShowCustomerOrders(User currentUser)
        {
            List<Order> orders= orderService.GetCustomerOrders(currentUser);
            DisplayOrders(orders);
        }

        public void ShowOrdersForSeller(User currentUser)
        {
            List<Order> orders = orderService.ShowSellerOrders(currentUser);
           DisplayOrders(orders);

        }

        public void DisplayOrders(List<Order> orders)
        {
            Console.WriteLine("===== Your Orders =====");
            if (orders.Count == 0)
            {
                Console.WriteLine("No orders placed yet..!!");
                return;
            }
            foreach (var order in orders)
            {
                Console.WriteLine($"Order ID: {order.OrderId}, Customer ID: {order.CustomerId}, Total Amount: {order.TotalAmount}, Order Date: {order.OrderDate}, Order Status : {order.Status}");
                Console.WriteLine("Ordered Product:");
                foreach (var product in order.ProductListToBeOrdered)
                {
                    Console.WriteLine($"Product ID: {product.Id}, Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}");
                    Console.WriteLine("______________________________________________________________");
                }
                Console.WriteLine("------------------------------------------------------------");
            }
        }

        public void UpdateOrderStatus(User currentUser)
        {
            Console.WriteLine("===== Update Order Status =====");

            List<Order> sellerOrders = orderService.ShowSellerOrders(currentUser);
            DisplayOrders(sellerOrders);

            int orderId = GetOrderIdFromUser();
            if (orderId == -1) return;

            Order orderToUpdate = orderService.GetOrderById(orderId);
            if (orderToUpdate == null)
            {
                Console.WriteLine("Order Not Found !!");
                return;
            }

            int statusChoice = GetOrderStatusChoice(orderToUpdate);
            if (statusChoice == -1) return;

            UpdateOrderStatus(orderToUpdate, statusChoice);
        }


        private int GetOrderIdFromUser()
        {
            Console.WriteLine("\nEnter the Order ID you want to update: ");
            if (int.TryParse(Console.ReadLine(), out int orderId))
            {
                return orderId;
            }
            else
            {
                Console.WriteLine("Invalid Input..");
                return -1;
            }
        }

        private int GetOrderStatusChoice(Order orderToUpdate)
        {
            Console.WriteLine("\nCurrent Status: " + orderToUpdate.Status);
            Console.WriteLine("\nAvailable Order Statuses:");

            foreach (var status in Enum.GetValues(typeof(OrderStatus)))
            {
                Console.WriteLine($"{(int)status} - {status}");
            }

            Console.Write("Enter the status number to update the order: ");
            if (int.TryParse(Console.ReadLine(), out int statusChoice))
            {
                return statusChoice;
            }
            else
            {
                Console.WriteLine("Invalid Input..");
                return -1;
            }
        }

        private void UpdateOrderStatus(Order orderToUpdate, int statusChoice)
        {
            orderService.UpdateOrderStatus(orderToUpdate, statusChoice);
            Console.WriteLine("Order Status is Updated !!!");
        }

        public async Task PlaceOrder(User currentUser)
        {
            Console.WriteLine("===== Place Order =====");

            Product selectedProduct = GetSelectedProduct();
            if (selectedProduct == null) return;

            decimal quantity = GetOrderQuantity(selectedProduct);
            if (quantity == 0) return;

            if (!ConfirmOrder(selectedProduct, quantity)) return;

            await ProcessOrder(selectedProduct, quantity, currentUser);
        }

        private Product GetSelectedProduct()
        {
            while (true)
            {
                Console.Write("\nEnter the Product ID you want to order: ");
                string input = Console.ReadLine();

                if (!int.TryParse(input, out int productId) || productId <= 0)
                {
                    Console.WriteLine("Invalid input. Please enter a valid numeric Product ID.");
                    continue;
                }

                Product product = productService.GetProductById(productId);
                if (product == null)
                {
                    Console.WriteLine("Invalid Product ID. Please try again.");
                    continue;
                }

                Console.WriteLine($"\nSelected Product: {product.Name}, Price: {product.Price:C}");
                return product;
            }
        }

        private decimal GetOrderQuantity(Product selectedProduct)
        {
            (decimal quantity, bool isValid) = ValidationHelper.GetValidatedDecimalInput("\nEnter the quantity you want to order: ");

            if (!isValid)
            {
                Console.WriteLine("Order Canceled.. Try again.");
                return 0;
            }

            if (quantity > selectedProduct.Quantity)
            {
                Console.WriteLine($"\nInsufficient stock. Only {selectedProduct.Quantity} units available. Please try again.");
                return 0;
            }

            return quantity;
        }

        private bool ConfirmOrder(Product selectedProduct, decimal quantity)
        {
            Console.WriteLine($"\n\n===== Order Summary =====");
            Console.WriteLine($"Product: {selectedProduct.Name}");
            Console.WriteLine($"Quantity: {quantity}");
            Console.WriteLine($"Total Price: {selectedProduct.Price * quantity:C}");

            Console.Write("Do you want to place the order? (yes to confirm): ");
            string confirmation = Console.ReadLine().ToLower();

            if (confirmation != "yes")
            {
                Console.WriteLine("Order canceled.");
                return false;
            }

            return true;
        }

        private async Task ProcessOrder(Product selectedProduct, decimal quantity, User currentUser)
        {
            try
            {
                selectedProduct.Quantity -= quantity;
                var orderedProducts = new List<Product>
        {
            new Product(selectedProduct.Id, selectedProduct.Name, selectedProduct.Description,
                        selectedProduct.Price, quantity, selectedProduct.SellerId)
        };

                await orderService.PlaceOrder(orderedProducts, currentUser);
                Console.WriteLine("Order placed successfully!");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error placing order: {e.Message}");
            }
        }


    }
}
