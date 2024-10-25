using ECommerceClassLibrary.Repositories;
using System.Collections.Generic;

namespace ECommerceClassLibrary.Models
{
    public class Customer : User
    {

        public List<string> Notifications { get; set; }
        public Customer() { }

        public Customer(int id, string name, string email, string password, string phoneNumber)
        {
            this.Email = email;
            this.UserId = id;
            this.Name = name;
            this.Password = password;
            this.PhoneNumber = phoneNumber;
            this.Role = UserRole.Customer;
            this.Notifications = new List<string>();
        }

        public override string ToString()
        {
            return $"Id: {UserId}, Name: {Name}, Email: {Email}, Phone: {PhoneNumber},  Role: {Role}";
        }


    }
}
