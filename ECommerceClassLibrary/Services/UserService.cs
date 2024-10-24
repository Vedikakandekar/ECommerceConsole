using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories;
using ECommerceClassLibrary.Repositories.Contracts;
using ECommerceClassLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using ECommerceClassLibrary.Helper;
using System.Text.RegularExpressions;

namespace ECommerceClassLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository repository;

        public UserService(IUserRepository repository)
        {

            this.repository = repository;
        }

        public Dictionary<string, string> GetUserDetails()
        {
            Dictionary<string, string> userParams = new Dictionary<string, string>();
            string nm ;
            Console.Write("Enter your name: ");
            while (true)
            {

                nm = Console.ReadLine();
                if(nm=="break")
                {
                    userParams["Name"] = null;
                    return userParams;
                }
                if (!string.IsNullOrEmpty(nm) & repository.IsUserNameUnique(nm) & ValidationHelper.IsAlphabetic(nm))
                {
                    userParams["Name"] = nm;
                    break;
                }
                Console.Clear();
                Console.WriteLine("Invalid Username ..Enter Valid Username containing characters only, it should be unique\ntype 'break' to go to main menu");
                Console.Write("Enter your name: ");
            }



            string email ;
            Console.Write("Enter valid email: ");
            while (true)
            {

                email = Console.ReadLine();
                if (email == "break")
                {
                    userParams["Email"] = null;
                    return userParams;
                }
                if (email.IsValidEmail())
                {
                    userParams["Email"] = email;
                    break;
                }
                Console.Clear();
                Console.WriteLine("Invalid Email ..Enter Valid Email..\ntype 'break' to go to main menu");
                Console.Write("Enter valid email: ");
            }

            string phone;
            Console.Write("Enter Valid phone number: ");
            while (true)
            {

                phone = Console.ReadLine();
                if (phone=="break")
                {
                    userParams["PhoneNumber"] = null;
                    return userParams;
                }
                if (phone.IsValidPhoneNumber())
                {
                    userParams["PhoneNumber"] = phone;
                    break;
                }
                Console.Clear();
                Console.WriteLine("Invalid Phone ..Enter Valid Phone Number...\ntype 'break' to go to main menu");
                Console.Write("Enter phone number: ");
            }

            Console.Write("Enter your password: ");
            userParams["Password"] = Console.ReadLine();

            return userParams;


        }

        public void SignUp(string userType)
        {
            Console.WriteLine("===== Sign Up =====");




            int uid = repository.GetUserCount() + 1;

            Dictionary<string, string> userParams = GetUserDetails();
            if (userParams["Name"]==null || userParams["Email"] == null || userParams["PhoneNumber"]==null)
            {
                System.Console.WriteLine("User Sign Up Failed...!!!");
                return;
            }
            User user;

            if (userType == "Customer")
            {
                user = UserFactory.CreateUser("Customer", userParams, uid);
                this.repository.CustomerSignUp(user);
            }
            else
                if (userType == "Admin")
            {
                user = UserFactory.CreateUser("Admin", userParams, uid);
                this.repository.CustomerSignUp(user);
            }
            else
                if (userType == "Seller")
            {
                user = UserFactory.CreateUser("Seller", userParams, uid);
                this.repository.CustomerSignUp(user);
            }


        }

        public void EditProfile(User currentCustomer, string field, dynamic newValue)
        {
            this.repository.EditProfile(currentCustomer, field, newValue);
        }

        public User GetUserById(int id)
        {
            return repository.GetUserById(id);
        }

        public void ShowProfile(User currentCustomer)
        {
            this.repository.ShowProfile(currentCustomer);
        }

        public (User, bool) ValidateUser(string nm, string pass, string fromController)
        {
            return repository.ValidateUser(nm, pass, fromController);
        }

        public void GetAllCustomers()
        {
            List<Customer> customers = repository.GetAllCustomers();
            System.Console.WriteLine("=======  Customers ======");
            if (customers.Count == 0)
            {
                System.Console.WriteLine("There are no customers yet..!!");
                return;
            }
            foreach (Customer customer in customers)
            {
                System.Console.WriteLine(customer.ToString());
            }

        }

        public void GetAllSellers()
        {
            List<Seller> sellers = repository.GetAllSellers();
            System.Console.WriteLine("=======  Sellers ======");
            if (sellers.Count == 0)
            {
                System.Console.WriteLine("There are no sellers yet..!!");
                return;
            }
            foreach (Seller seller in sellers)
            {
                System.Console.WriteLine(seller.ToString());
            }
        }

        public void GetAllUsers()
        {
            List<User> users = repository.GetAllUsers();
            System.Console.WriteLine("=======  All Users ======");
            if (users.Count == 0)
            {
                System.Console.WriteLine("There are no users yet..!!");
                return;
            }
            foreach (User seller in users)
            {
                System.Console.WriteLine(seller.ToString());
            }
        }

        public void DeleteUser(User user)
        {


            repository.DeleteUser(user);
            System.Console.WriteLine("User Deleted Successfully !!!..");

        }
    }



}
