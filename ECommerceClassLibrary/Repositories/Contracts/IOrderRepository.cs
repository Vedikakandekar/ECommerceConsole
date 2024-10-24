using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerceClassLibrary.Repositories.Contracts
{
    public interface IOrderRepository
    {

        Task PlaceOrder(List<Product> orderedProducts, Order newOrder);

        List<Order> ShowCustomerOrders(User currentUser);

        List<Order> ShowAllOrders();

        bool UpdateOrderStatus(Order order, int statusChoice);

        bool IsOrderListEmpty();

        Order GetOrderById(int id);

        int GetOrderCount();


    }
}
