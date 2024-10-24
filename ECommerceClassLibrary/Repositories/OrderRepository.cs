using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceClassLibrary.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private static List<Order> _orders;
      


        static OrderRepository()
        {
            _orders = new List<Order>();
        }

        public async Task PlaceOrder(List<Product> orderedProducts, Order newOrder)
        {
            await Task.Run(async () =>
            {
                _orders.Add(newOrder);
              //  await Task.Delay(2000);
                Console.WriteLine(newOrder.ToString());

            });
        }


        public int GetOrderCount()
        { return _orders.Count; }


        public List<Order> ShowCustomerOrders(User currentCustomer)
        {
            var customerOrders = _orders.Where(o => o.CustomerId == currentCustomer.UserId).ToList();
            return customerOrders;
        }

        public List<Order> ShowAllOrders()
        {
            if (IsOrderListEmpty())
            {
               // Console.WriteLine("There aren't any Orders Placed yet.");
                return new List<Order>();
            }

            return _orders;
        }

        public bool IsOrderListEmpty()
        {

            if (_orders.Count == 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }

        public bool UpdateOrderStatus(Order orderToUpdate, int statusChoice)
        {

            if (Enum.IsDefined(typeof(OrderStatus), statusChoice))
            {
                _orders.FirstOrDefault(o => o.OrderId == orderToUpdate.OrderId).Status = (OrderStatus)statusChoice;
                Console.WriteLine($"Order {orderToUpdate.OrderId} status updated to {orderToUpdate.Status}.");
                return true;
            }
            else
            {
                Console.WriteLine("Invalid status choice.");
                return false;
            }
        }

        public Order GetOrderById(int id)
        {
            Order order = _orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null)
            {
                Console.WriteLine("Invalid Order ID.");
                return new Order();
            }
            return order;


        }

    }
}
