using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories;
using ECommerceClassLibrary.Repositories.Contracts;
using ECommerceClassLibrary.Services;
using ECommerceClassLibrary.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ECommerceClassLibrary.Controllers
{
    public class UserController
    {

        private readonly IUserService userService;

        private readonly IProductService productService;

        private readonly IOrderService orderService;
        public UserController(IUserService userService,IProductService productService, IOrderService orderService)
        {
            this.userService = userService;
            this.productService = productService;
            this.orderService = orderService;
        }

        public void AddUser(string userType)
        {
            this.userService.SignUp(userType);
        }

        public (User, bool) ValidateUser(string nm, string pass, string userType)
        {
            return ((User, bool))userService.ValidateUser(nm, pass, userType);
        }



        public void ShowProfile(User currentCustomer)
        {
            userService.ShowProfile(currentCustomer);
        }

        public void EditProfile(User currentCustomer, string feild, dynamic newValue)
        {
            userService.EditProfile(currentCustomer, feild, newValue);
        }

        public void GetAllCustomer(User currentUser)
        {
            if (currentUser.Role == "Admin") {
                userService.GetAllCustomers();
            }
            else
            {
                Console.WriteLine("Access denied !!");
            }
        }
        public void GetAllSellers(User currentUser)
        {
            if (currentUser.Role == "Admin")
            {
                userService.GetAllSellers();
            }
            else
            {
                Console.WriteLine("Access denied !!");
            }
        }
        public void GetAllUsers(User currentUser)
        {
            if (currentUser.Role == "Admin")
            {
                userService.GetAllUsers();
            }
            else
            {
                Console.WriteLine("Access denied !!");
            }
        }

        public void DeleteUser()
        {

            userService.GetAllUsers();
            

            System.Console.WriteLine("Enter UserId to delete the User");
            string str = Console.ReadLine();
            if (!int.TryParse(str, out int userId))
            {
                System.Console.WriteLine("Invalid choice...!!");
                return;
            }

            else
            {
                User user = userService.GetUserById(userId);
                if (user == null)
                {
                    System.Console.WriteLine("User Does Not Exist..!!");
                }

                else
                 if (user.Role == "Admin")
                {
                    System.Console.WriteLine("Cannot Remove Admin..!!");
                }
                else
                {

                    if (user.Role == "Seller")
                    {
                        List<Product> sellerProducts = productService.GetSellerProducts(user);

                        List<Order> sellerOrders = orderService.ShowSellerOrders(user);
                        bool hasPendingOrders = sellerOrders.Any(o => o.ProductListToBeOrdered.Any(p => sellerProducts.Contains(p)) &&
                                                                      (o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Canceled));

                        if (hasPendingOrders)
                        {
                            System.Console.WriteLine("Cannot delete seller. The seller has products in orders that are not delivered or canceled.");
                            return;
                        }
                    }
                    else if (user.Role == "Customer")
                    {
                        List<Order> customerOrders = orderService.GetCustomerOrders(user);
                        bool hasPendingOrders = customerOrders.Any(o => o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Canceled);

                        if (hasPendingOrders)
                        {
                            System.Console.WriteLine("Cannot delete customer. The customer has pending orders that are not delivered or canceled.");
                            return;
                        }
                    }


                    userService.DeleteUser(user);

                }
            }

        }
    }
}
