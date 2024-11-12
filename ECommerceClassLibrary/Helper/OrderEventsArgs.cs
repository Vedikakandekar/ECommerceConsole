using ECommerceClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceClassLibrary.Helper
{
    public class OrderEventArgs : EventArgs
    {
        public int OrderId { get; }
        public int CustomerId { get; }
        public OrderStatus Status { get; }

        public OrderEventArgs(int orderId, int customerId, OrderStatus status)
        {
            OrderId = orderId;
            CustomerId = customerId;
            Status = status;
        }
    }

}
