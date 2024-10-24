using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceClassLibrary.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<User> Users;


        static UserRepository()
        {
            Users = new List<User>()
            {
                new Admin(103,"admin","asd@ghh.com","admin","7878787878"),
                new Customer(102,"cust","ved@ved.com","cust","9898989898"),
                new Seller(101,"sell","ved@ved.com","sell","8787878787")
            };
        }


        public bool DeleteUser(User user)
        {

            if (user != null)
            {
                
          return Users.Remove(user);
            }
            return false;
        }

        public User GetUserById(int id)
        {
            foreach (var user in Users)
            {
                if (user.UserId == id)
                    return user;
            }

            return null;
        }

        public (User, bool) ValidateUser(string unm, string pass, string userType)
        {
           
            foreach (var user in Users)
            {
                if (user.Name == unm & user.Password == pass & user.Role == userType)
                {

                    return (user, true);
                }
            }
            return (null, false);
        }



        public void CustomerSignUp(User user)
        {

            Users.Add(user);

            Console.WriteLine("Customer signed up successfully!");


        }

        public int GetUserCount()
        {
            return Users.Count();
        }

        public void ShowProfile(User currentCustomer)
        {
            Console.WriteLine("===== Your Profile =====");
            Console.WriteLine(currentCustomer.ToString()); 
        }

        public void EditProfile(User currentCustomer, string field, dynamic newValue)
        {
            if (field == "Name")
            {
                currentCustomer.Name = newValue;
                Console.WriteLine("Name updated successfully.");
            }
            else if (field == "Email")
            {
                currentCustomer.Email = newValue;
                Console.WriteLine("Email updated successfully.");

            }
            else if (field == "Password")
            {
                currentCustomer.Password = newValue;
                Console.WriteLine("Password updated successfully.");
            }
            else if (field == "Phone")
            {
                currentCustomer.PhoneNumber = newValue;
                Console.WriteLine("Phone Number updated successfully.");
            }
            else
            { Console.WriteLine("Invalid Choice"); }
        }

        public bool IsUserNameUnique(string userName)
        {
            return Users.All(u => u.Name != userName);
        }

        public List<Customer> GetAllCustomers()
        {
            return Users.OfType<Customer>().ToList();
        }

        public List<Seller> GetAllSellers()
        {
            return Users.OfType<Seller>().ToList();
        }

        public List<User> GetAllUsers()
        {
            return Users;
        }
    }



}
