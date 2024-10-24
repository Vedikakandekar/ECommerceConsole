using ECommerceClassLibrary.Repositories;
using System;
using System.Collections.Generic;

namespace ECommerceClassLibrary.Models
{
    public static class UserFactory
    {
        public static User CreateUser(string userType, Dictionary<string, string> userParams, int uid)
        {
            switch (userType.ToLower())
            {
                case "customer":
                    return new Customer
                    {
                        UserId = uid,
                        Name = userParams["Name"],
                        Email = userParams["Email"],
                        Password = userParams["Password"],
                        PhoneNumber = userParams["PhoneNumber"],
                        Role = "Customer",
                    };

                case "admin":
                    return new Admin
                    {

                        UserId = uid,
                        Name = userParams["Name"],
                        Email = userParams["Email"],
                        Password = userParams["Password"],
                        PhoneNumber = userParams["PhoneNumber"],
                        Role = "Admin",
                    };

                case "seller":
                    return new Seller
                    {

                        UserId = uid,
                        Name = userParams["Name"],
                        Email = userParams["Email"],
                        Password = userParams["Password"],
                        PhoneNumber = userParams["PhoneNumber"],
                        Role = "Seller",
                    };

                default:
                    throw new ArgumentException("Invalid user type provided.");
            }
        }
    }
}
