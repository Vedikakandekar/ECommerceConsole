using System;
using System.Collections.Generic;

namespace ECommerceClassLibrary.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public List<Product> ProductListToBeOrdered { get; set; }
        // public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        public Address ShippingAddress { get; set; }
        public OrderStatus Status { get; set; }

        public Order(int orderId, int customerId, List<Product> productList, decimal totalAmount, Address shippingAddress,OrderStatus status)
        {
            OrderId = orderId;
            CustomerId = customerId;
            ProductListToBeOrdered = productList;
            TotalAmount = totalAmount;
            OrderDate = DateTime.Now;
            ShippingAddress = shippingAddress;
            Status = status;
        }

        public override string ToString()
        {
            return $"Order ID: {OrderId}, Total Amount: {TotalAmount:C}, Date: {OrderDate}, Status: {Status}, \nShipping Address: {ShippingAddress}, \nProduct Name: {ProductListToBeOrdered[0].Name}, Quantity: {ProductListToBeOrdered[0].Quantity}";
        }
        public Order(){}
    }


    public enum OrderStatus
    {
        Pending,
        Processed,
        Shipped,
        Delivered,
        Canceled
    }


    public struct Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }


        public Address(string street, string city, string state, string zipCode)
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;

        }

        public override string ToString()
        {
            return $"Address : {Street}, {City}, {State}, {ZipCode}";
        }
    }

}
