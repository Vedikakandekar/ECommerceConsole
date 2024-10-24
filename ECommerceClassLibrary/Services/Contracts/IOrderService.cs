using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceClassLibrary.Services.Contracts
{
    public interface IOrderService
    {

        event OrderEventHandler OrderProcessed;
        Task PlaceOrder(List<Product> orderedProduct, User currentUser);
        List<Order> ShowAllOrders();
        List<Order> GetCustomerOrders(User currentUser);

        List<Order> ShowSellerOrders(User currentUser);
        void UpdateOrderStatus(Order order, int statusChoice);

        bool IsOrderListEmpty();

        Order GetOrderById(int id);
    }
}
