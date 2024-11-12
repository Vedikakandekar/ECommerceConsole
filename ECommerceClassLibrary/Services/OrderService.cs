using ECommerceClassLibrary.Helper;
using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories;
using ECommerceClassLibrary.Repositories.Contracts;
using ECommerceClassLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceClassLibrary.Services
{
    public delegate void OrderEventHandler(object sender, OrderEventArgs e);

    public class OrderService : IOrderService
    {

        private readonly IOrderRepository repository;

        public event OrderEventHandler OrderProcessed;

        public OrderService(IOrderRepository repository)
        {
            this.repository = repository;
        }

        public Address GetShippingAddress()
        {
            Console.WriteLine("Enter Shipping Address:");


            (string street, bool boolstreet) = ValidationHelper.GetValidatedStringInput("Street : ");
            if (!boolstreet)
            {

                return new Address("", "", "", "");
            }

            (string city, bool boolCity) = ValidationHelper.GetValidatedStringInput("City : ");
            if (!boolCity)
            {
                return new Address("", "", "", "");
            }


            (string state, bool boolState) = ValidationHelper.GetValidatedStringInput("State : ");
            if (!boolState)
            {
                return new Address("", "", "", "");
            }

            (string zipCode, bool boolCode) = ValidationHelper.IsValidPinCode("ZipCode  : ");
            if (!boolCode)
            {
                return new Address("", "", "", "");
            }
            return new Address(street, city, state, zipCode);

        }

        public decimal CalculateTotalAmount(List<Product> orderedProducts)
        {
            return orderedProducts.Sum(product => product.Price);
        }



        public async Task PlaceOrder(List<Product> orderedProducts, User currentUser)
        {

            if (orderedProducts == null || orderedProducts.Count == 0)
            {
                Console.WriteLine("No products selected. Order not placed.");
                return;
            }
            decimal totalAmount = CalculateTotalAmount(orderedProducts);

            Address shippingAddress = GetShippingAddress();

            if (string.IsNullOrEmpty(shippingAddress.Street ) || string.IsNullOrEmpty(shippingAddress.City)|| string.IsNullOrEmpty(shippingAddress.State)|| string.IsNullOrEmpty(shippingAddress.ZipCode))
            {
                Console.WriteLine("Shipping Address is not given. Order not placed.");
                return;
            }

            Order newOrder = new Order(repository.GetOrderCount() + 1, currentUser.UserId, orderedProducts, totalAmount, shippingAddress, OrderStatus.Pending);

            Console.WriteLine("\nProcessing your order, please wait...");
            try
            {
                //await Task.Delay(2000);
                await repository.PlaceOrder(orderedProducts, newOrder);
                Console.WriteLine("Order placed successfully!");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.ToString());
            }

        }

        public int GetOrderCount()
        { return repository.GetOrderCount(); }

        public List<Order> ShowAllOrders()
        {
            return this.repository.ShowAllOrders();
        }

        public List<Order> GetCustomerOrders(User currentUser)
        {
            List<Order> OrderList = repository.ShowCustomerOrders(currentUser);

            if (OrderList.Count == 0)
            {
                return OrderList;
            }
            var sortedOrders = OrderList.OrderBy(order => order.OrderDate).ToList();
            //foreach (var order in sortedOrders)
            //{
            //    Console.WriteLine(order.ToString());
            //}

            return sortedOrders;
        }


        public void UpdateOrderStatus(Order orderToUpdate, int statusChoice)
        {
            if (orderToUpdate.Status == OrderStatus.Delivered || orderToUpdate.Status == OrderStatus.Canceled)
            {
                Console.WriteLine("Order is " + orderToUpdate.Status + " Cannot change status");
                return;
            }
            if (repository.UpdateOrderStatus(orderToUpdate, statusChoice))
            {
                OrderProcessed?.Invoke(this, new OrderEventArgs(orderToUpdate.OrderId, orderToUpdate.CustomerId, orderToUpdate.Status));
            }
        }
        public bool IsOrderListEmpty()
        {
            return repository.IsOrderListEmpty();
        }

        public Order GetOrderById(int id)
        {
            Order o = repository.GetOrderById(id);
            return o ?? throw new OrderNotFoundException(id);
        }

        public List<Order> ShowSellerOrders(User currentUser)
        {
            List<Order> allOrders = repository.ShowAllOrders();
            List<Order> sellerOrders = new List<Order>();

            foreach (var order in allOrders)
            {
                List<Product> sellerProductsInOrder = order.ProductListToBeOrdered
                                                   .Where(p => p.SellerId == currentUser.UserId)
                                                   .ToList();

                if (sellerProductsInOrder.Count > 0)
                {
                    sellerOrders.Add(new Order(order.OrderId, order.CustomerId, sellerProductsInOrder, sellerProductsInOrder.Sum(p => p.Price * p.Quantity), order.ShippingAddress, order.Status));
                }
            }

            return sellerOrders;
        }
    }

    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException()
            : base("Order not found.")
        {
        }

        public OrderNotFoundException(int orderId)
            : base($"Order with ID {orderId} not found.")
        {
        }
    }
}
