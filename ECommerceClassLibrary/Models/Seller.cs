using ECommerceClassLibrary.Repositories;

namespace ECommerceClassLibrary.Models
{
    public class Seller : User
    {
        public Seller() { }

        public Seller(int id, string name, string email, string password,string phone)
        {
            this.Email = email;
            this.UserId = id;
            this.Name = name;
            this.Password = password;
            Role = "Seller";
            this.PhoneNumber = phone;
        }

        public override string ToString()
        {
            return $"Id:  {UserId} , Name:  {Name} , Email:  {Email} , Phone:  {PhoneNumber} ,  Role:  {Role}";
        }
    }
}
