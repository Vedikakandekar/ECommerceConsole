using ECommerceClassLibrary.Repositories;
using System;
using System.Collections.Generic;

namespace ECommerceClassLibrary.Models
{
    public static class UserFactory
    {
        public static User CreateUser(UserRole userType, Dictionary<string, string> userParams, int uid)
        {
            switch (userType)
            {
                case UserRole.Customer:
                    return new Customer
                    {
                        UserId = uid,
                        Name = userParams["Name"],
                        Email = userParams["Email"],
                        Password = userParams["Password"],
                        PhoneNumber = userParams["PhoneNumber"],
                        Role = UserRole.Customer,
                        Notifications = new List<string>()
                    };

                case UserRole.Admin:
                    return new Admin
                    {

                        UserId = uid,
                        Name = userParams["Name"],
                        Email = userParams["Email"],
                        Password = userParams["Password"],
                        PhoneNumber = userParams["PhoneNumber"],
                        Role = UserRole.Admin,
                    };

                case UserRole.Seller:
                    return new Seller
                    {

                        UserId = uid,
                        Name = userParams["Name"],
                        Email = userParams["Email"],
                        Password = userParams["Password"],
                        PhoneNumber = userParams["PhoneNumber"],
                        Role = UserRole.Seller,
                    };

                default:
                    throw new ArgumentException("Invalid user type provided.");
            }
        }
    }
}
