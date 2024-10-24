
using ECommerce.Repositories;
using ECommerceAssignment;
using ECommerceClassLibrary.Controllers;
using ECommerceClassLibrary.Helper;
using ECommerceClassLibrary.Models;
using ECommerceClassLibrary.Repositories;
using ECommerceClassLibrary.Repositories.Contracts;
using ECommerceClassLibrary.Services;
using ECommerceClassLibrary.Services.Contracts;
using Microsoft.Extensions.DependencyInjection;
namespace ECommerceAssignment
{
    public class Program
    {
        static UserController? userController;
        static ProductController? productController;
        static OrderController? orderController;
         static Program()
        {
            try
            {
                Logger.LogInfo("Application started.");
                var serviceProvider = new ServiceCollection()
                         .AddSingleton<IUserRepository, UserRepository>()
                         .AddSingleton<IProductRepository, ProductRepository>()
                         .AddSingleton<IOrderRepository, OrderRepository>()
                         .AddSingleton<IUserService, UserService>()
                         .AddSingleton<IProductService, ProductService>()
                        .AddSingleton<IOrderService, OrderService>()
                         .AddSingleton<UserController>()
                          .AddSingleton<OrderController>()
                          .AddSingleton<ProductController>()
                         .BuildServiceProvider();

                userController = serviceProvider.GetService<UserController>();
                productController = serviceProvider.GetService<ProductController>();
                orderController = serviceProvider.GetService<OrderController>();

                
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred : {ex.Message} \n{ex.StackTrace}");
            }
        }

        public async static Task Main()
        {
            try
            {
                Program program = new();
                if (userController != null && productController != null && orderController != null)
                {
                    await BaseMenu();
                    Logger.LogInfo("Application finished successfully.");
                }
                else
                {
                    Logger.LogInfo("Dependancy Injection Failed..");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred : {ex.Message} \nStack Trace :{ex.StackTrace}");
            }
        }

        static async Task BaseMenu()
        {
            try
            {
                if (userController != null && productController != null && orderController != null)
                {


                   Console.Clear();
                    string str = """
            ===== Main Menu =====
            1. Customer
            2. Seller
            3. Admin
            4. Exit
            """;

                    int ch = ValidationHelper.GetValidatedNumberInput(str);


                    switch (ch)
                    {
                        case 1:
                            await SignUpSignInMenu("Customer");
                            break;

                        case 2:
                            await SignUpSignInMenu("Seller");
                            break;

                        case 3:
                            await SignUpSignInMenu("Admin");
                            break;

                        case 4:
                            Console.WriteLine("Exiting Application !!");
                            Logger.LogInfo("Application finished successfully.");
                            Environment.Exit(0);
                            break;



                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            Console.WriteLine("Press any key to continue..!!");
                            Console.ReadKey();
                            await BaseMenu();
                            break;
                    }

                }
                else
                {
                    Logger.LogError("\nDependancy Injection Failed");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred : {ex.Message} \n{ex.StackTrace}");
            }

        }

        static async Task SignUpSignInMenu(string userType)
        {
            try
            {
                if (userController != null && productController != null && orderController != null)
                {


                    string str = "===== Welcome ===== \n1. Sign Up\n2. Sign In\n3.Back to Main Menu\n4.Exit";
                    int customerChoice = ValidationHelper.GetValidatedNumberInput(str);
                    switch (customerChoice)
                    {
                        case 1:
                            if (userType == "Customer")
                            {


                                userController.AddUser(userType);
                            }
                            else if (userType == "Seller")
                            {
                                userController.AddUser(userType);
                            }
                            else if (userType == "Admin")
                            {
                                userController.AddUser(userType);
                            }
                            Console.WriteLine("Please SignIn with your name and password.");
                            Console.WriteLine("Press any key to continue..!!");
                            Console.ReadKey();

                            await BaseMenu();
                            break;
                            
                        case 2:

                            (string unm1,bool b1) = ValidationHelper.GetValidUserName("Enter Username  : ");
                            if(!b1)
                            {
                                await BaseMenu();
                            }

                            (string pass1,bool b2) = ValidationHelper.GetValidUserName("Enter Password : ");
                            if(!b2)
                            {
                                await BaseMenu();
                            }

                            if (userType == "Customer")
                            {
                                (User currentUser, bool validate) = userController.ValidateUser(unm1, pass1, "Customer");
                                if (validate)
                                {
                                    if (currentUser is Customer cust)
                                    {
                                        if (cust.notifications.Count != 0)
                                        {
                                            Console.WriteLine("======= Updates =======");
                                            foreach (string s in cust.notifications)
                                            {
                                                Console.WriteLine(s);
                                            }
                                            cust.notifications.Clear();
                                            Console.WriteLine("Press any key to continue !!..");
                                            Console.ReadKey();
                                        }
                                    }

                                    await ShowCustomerActionsMenu(currentUser);

                                }
                                else
                                {
                                    Console.WriteLine("Invalid UserName or Password.. Please try again !!");
                                    Console.WriteLine("Press any key to continue..!!");
                                    Console.ReadKey();

                                    await BaseMenu();
                                }
                            }
                            else
                                if (userType == "Seller")
                            {
                                (User currentUser, bool validate) = userController.ValidateUser(unm1, pass1, "Seller");
                                if (validate)
                                {
                                    await ShowSellerMenu(currentUser);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid UserName or Password.. Please try again !!");
                                    Console.WriteLine("Press any key to continue ..");
                                    Console.ReadKey();
                                    await BaseMenu();
                                }
                            }
                            else
                                if (userType == "Admin")
                            {
                                (User currentUser, bool validate) = userController.ValidateUser(unm1, pass1, "Admin");
                                if (validate)
                                {
                                    await ShowAdminMenu(currentUser);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid UserName or Password.. Please try again !!");
                                    Console.WriteLine("Press any key to continue..!!");
                                    Console.ReadKey();
                                    await BaseMenu();
                                }
                            }
                            break;

                        case 3:
                            await BaseMenu();
                            break;
                        case 4:
                            Console.WriteLine("Exiting Application !!");
                            Logger.LogInfo("Application finished successfully.");
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            await SignUpSignInMenu(userType);
                            break;
                    }
                }
                else
                {
                    Logger.LogError("\nDependancy Injection Failed");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred : {ex.Message} \n{ex.StackTrace}");
            }



        }


        private async static Task ShowAdminMenu(User currentUser)
        {

            try
            {
                if (userController != null && productController != null && orderController != null)
                {

                    int actionChoice;
                    do
                    {

                        Console.Clear();
                        string str = """
                    ===== Admin Menu =====
                    1. Show  Products
                    2. Show Orders
                    3. Show Customers
                    4. Show Sellers
                    5. Delete User
                    6. Logout
                    7. Exit Application
                    Enter your choice:
                    """;

                        actionChoice = ValidationHelper.GetValidatedNumberInput(str);

                        switch (actionChoice)
                        {
                            case 1:
                                productController.ShowAllProducts();
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;


                            case 2:
                                orderController.ShowAllOrders();
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;

                            case 3:
                                userController.GetAllCustomer(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;

                            case 4:
                                userController.GetAllSellers(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();

                                break;

                            case 5:
                                userController.DeleteUser();
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;

                            case 6:
                                Console.WriteLine("Logging out...");
                                await BaseMenu();

                                break;

                            case 7:
                                Console.WriteLine("Exiting Application !!");
                                Logger.LogInfo("Application finished successfully.");
                                Environment.Exit(0);
                                break;

                        }


                    } while (actionChoice != 7);

                }
                else
                {
                    Logger.LogError("\nDependancy Injection Failed");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred : {ex.Message} \n{ex.StackTrace}");
            }


        }

        static async Task ShowCustomerActionsMenu(User currentUser)
        {
            try
            {
                if (userController != null && productController != null && orderController != null)
                {


                    int actionChoice;
                    do
                    {
                        Console.Clear();
                        string str = """
                    ===== Customer Menu =====
                    1. Show Products
                    2. Place Order
                    3. View Orders
                    4. Show Profile
                    5. Edit Profile
                    6. Logout
                    7. Exit Application
                    Enter your choice:
                    """;

                        actionChoice = ValidationHelper.GetValidatedNumberInput(str);

                        switch (actionChoice)
                        {
                            case 1:
                                productController.ShowAllProducts();
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;
                            case 2:
                                productController.ShowAllProducts();
                                await orderController.PlaceOrder(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;
                            case 3:
                                orderController.ShowCustomerOrders(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;
                            case 4:
                                userController.ShowProfile(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;
                            case 5:
                                EditProfile(currentUser);
                                break;
                            case 6:
                                Console.WriteLine("Logging out...");
                                await BaseMenu();
                                break;

                            case 7:
                                Console.WriteLine("Exiting Application !!");
                                Logger.LogInfo("Application finished successfully.");
                                Environment.Exit(0);
                                break;


                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                await ShowCustomerActionsMenu(currentUser);
                                break;
                        }
                    } while (actionChoice != 7);

                }
                else
                {
                    Logger.LogError("\nDependancy Injection Failed");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred : {ex.Message} \n{ex.StackTrace}");
            }

        }

        private static void EditProfile(User currentUser)
        {
            try
            {
                if (userController != null && productController != null && orderController != null)
                {

                    string str = """
            ===== Edit Your Profile =====
            1. Edit Name
            2. Edit Email
            3. Edit Password
            4. Edit Phone Number
            5. Back
            Enter your choice:
            """;


                    int choice = ValidationHelper.GetValidatedNumberInput(str);

                    switch (choice)
                    {
                        case 1:
                            (string newName, bool b1) = ValidationHelper.GetValidUserName("Enter new name: ");
                            if (!b1)
                                break;
                            userController.EditProfile(currentUser, "Name", newName);

                            Console.WriteLine("Press any key to continue..!!");
                            Console.ReadKey();
                            break;

                        case 2:                          
                            (string newEmail, bool b2) = ValidationHelper.GetValidatedEmail("Enter new email: ");
                            if (!b2)
                                break;
                            userController.EditProfile(currentUser, "Email", newEmail);

                            Console.WriteLine("Press any key to continue..!!");
                            Console.ReadKey();
                            break;

                        case 3:
                            (string currentPassword,bool b3) = ValidationHelper.GetValidUserName("Enter  current password: ");
                            if (!b3)
                                break;
                            if (currentPassword == currentUser.Password)
                            {
                                Console.Write("Enter new password: ");
                                (string newPassword,bool b5) = ValidationHelper.GetValidatedStringInput("Enter new password: ");
                                if (!b5)
                                    break;
                                userController.EditProfile(currentUser, "Password", newPassword);
                            }
                            else
                            {
                                Console.WriteLine("Incorrect current password. Try again !!");
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                EditProfile(currentUser);
                            }

                            break;

                        case 4:

                            (string newPhone,bool b4) = ValidationHelper.GetValidUserName("Enter new Phone Number: ");
                            if (!b4)
                                break;
                            userController.EditProfile(currentUser, "Phone", newPhone);

                            Console.WriteLine("Press any key to continue..!!");
                            Console.ReadKey();
                            break;

                        case 5:
                            break;




                        default:
                            Console.WriteLine("Invalid choice.");
                            Console.WriteLine("Press any key to continue..!!");
                            Console.ReadKey();
                            EditProfile(currentUser);
                            break;
                    }

                }
                else
                {
                    Logger.LogError("\nDependancy Injection Failed");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred : {ex.Message} \n{ex.StackTrace}");
            }


        }

        async static Task ShowSellerMenu(User currentUser)
        {
            try
            {
                if (userController != null && productController != null && orderController != null)
                {

                    int adminChoice;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("===== Seller Menu =====");
                        Console.WriteLine("1. Add Product");
                        Console.WriteLine("2. Update Product");
                        Console.WriteLine("3. Update Product Quantity");
                        Console.WriteLine("4. Show Orders");
                        Console.WriteLine("5. Show Products");
                        Console.WriteLine("6. Update Order Status");
                        Console.WriteLine("7. Delete Product");
                        Console.WriteLine("8. Show Profile");
                        Console.WriteLine("9. Edit Profile");
                        Console.WriteLine("10. Logout");
                        Console.WriteLine("11. Exit Application");
                        Console.WriteLine("Enter your choice:");

                        if (!int.TryParse(Console.ReadLine(), out adminChoice))
                        {
                            Console.WriteLine("Invalid input. Please enter a valid number.");
                            continue;
                        }

                        switch (adminChoice)
                        {
                            case 1:
                                productController.AddProduct(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                break;
                            case 2:
                                productController.UpdateProduct(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                break;
                            case 3:
                                productController.UpdateProductQuantity(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                break;
                            case 4:
                                orderController.ShowOrdersForSeller(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                break;
                            case 5:
                                productController.ShowSellerProducts(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                break;

                            case 6:
                                orderController.UpdateOrderStatus(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                break;

                            case 7:
                                productController.DeleteProduct(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                break;

                            case 8:
                                userController.ShowProfile(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                System.Console.ReadKey();
                                break;

                            case 9:
                                EditProfile(currentUser);
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                break;


                            case 10:
                                Console.WriteLine("Logging out...");
                                await BaseMenu();
                                break;
                            case 11:
                                Console.WriteLine("Exiting Application !!");
                                Logger.LogInfo("Application finished successfully.");
                                Environment.Exit(0);
                                break;
                            default:
                                Console.WriteLine("Invalid choice. Please try again.");
                                Console.WriteLine("Press any key to continue..!!");
                                Console.ReadKey();
                                await ShowSellerMenu(currentUser);
                                break;
                        }
                    } while (adminChoice != 11);

                }
                else
                {
                    Logger.LogError("\nDependancy Injection Failed");
                }
            }
            catch (Exception ex)
            {
                Logger.LogError($"Exception occurred : {ex.Message} \n{ex.StackTrace}");
            }



        }
    }
}