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

        public void AddUser(UserRole userType)
        {
            this.userService.SignUp(userType);
        }

        public (User, bool) ValidateUser(string nm, string pass, UserRole userType)
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
            if (currentUser.Role == UserRole.Admin) {
                userService.GetAllCustomers();
            }
            else
            {
                Console.WriteLine("Access denied !!");
            }
        }

        public void GetAllSellers(User currentUser)
        {
            if (currentUser.Role == UserRole.Admin)
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
            if (currentUser.Role == UserRole.Admin)
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

            int userId = GetUserIdForDeletion();
            if (userId == -1)
            {
                Console.WriteLine("Invalid UserId. Deletion aborted.");
                return;
            }

            User user = userService.GetUserById(userId);
            if (user == null)
            {
                Console.WriteLine("User does not exist.");
                return;
            }

            if (!CanDeleteUser(user))
            {
                return;
            }

            userService.DeleteUser(user);
            Console.WriteLine("User deleted successfully.");
        }

        private int GetUserIdForDeletion()
        {
            Console.WriteLine("Enter UserId to delete the user:");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int userId))
            {
                return userId;
            }

            return -1;
        }

        private bool CanDeleteUser(User user)
        {
            if (user.Role == UserRole.Admin)
            {
                Console.WriteLine("Cannot delete admin.");
                return false;
            }

            if (user.Role == UserRole.Seller && HasPendingSellerOrders(user))
            {
                Console.WriteLine("Cannot delete seller with pending orders.");
                return false;
            }

            if (user.Role == UserRole.Customer && HasPendingCustomerOrders(user))
            {
                Console.WriteLine("Cannot delete customer with pending orders.");
                return false;
            }

            return true;
        }

        private bool HasPendingSellerOrders(User seller)
        {
            List<Product> sellerProducts = productService.GetSellerProducts(seller);
            List<Order> sellerOrders = orderService.ShowSellerOrders(seller);

            return sellerOrders.Any(order =>
                order.ProductListToBeOrdered.Any(product => sellerProducts.Contains(product)) &&
                order.Status != OrderStatus.Delivered && order.Status != OrderStatus.Canceled);
        }

        private bool HasPendingCustomerOrders(User customer)
        {
            List<Order> customerOrders = orderService.GetCustomerOrders(customer);

            return customerOrders.Any(order =>
                order.Status != OrderStatus.Delivered && order.Status != OrderStatus.Canceled);
        }

    }
}
