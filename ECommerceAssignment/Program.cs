
using ECommerce.Repositories;
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
                InitializeServices();
                Logger.LogInfo("Application started.");
            }
            catch (Exception ex)
            {
                Logger.LogError($"Initialization failed: {ex.Message} \n{ex.StackTrace}");
            }
        }
        public async static Task Main()
        {
            if (ControllersInitialized())
            {
                await ShowMainMenu();
                Logger.LogInfo("Application finished successfully.");
            }
            else
            {
                Logger.LogInfo("Dependency Injection Failed.");
            }
        }


        private static void InitializeServices()
        {
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

        private static bool ControllersInitialized() =>
       userController != null && productController != null && orderController != null;

        static async Task ShowMainMenu()
        {
            int choice;
            do
            {
                Console.Clear();
                Console.WriteLine("""
                ===== Main Menu =====
                1. Customer
                2. Seller
                3. Admin
                4. Exit
                """);
                choice = ValidationHelper.GetValidatedNumberInput("Enter choice: ");
                await HandleMainChoice(choice);

            } while (choice != 4);
        }

        private static async Task HandleMainChoice(int choice)
        {
            switch (choice)
            {
                case 1:
                    await ShowSignUpSignInMenu(UserRole.Customer);
                    break;
                case 2:
                    await ShowSignUpSignInMenu(UserRole.Seller);
                    break;
                case 3:
                    await ShowSignUpSignInMenu(UserRole.Admin);
                    break;
                case 4:
                    Console.WriteLine("Exiting Application...");
                    Logger.LogInfo("Application exited by user.");
                    Environment.Exit(0);
                    break;
                default:
                    await InvalidChoice();
                    break;
            }
        }

        static async Task ShowSignUpSignInMenu(UserRole userType)
        {
            Console.Clear();
            Console.WriteLine("""
            ===== Welcome =====
            1. Sign Up
            2. Sign In
            3. Back to Main Menu
            4. Exit
            """);
            int choice = ValidationHelper.GetValidatedNumberInput("Enter choice: ");
            switch (choice)
            {
                case 1:
                    await SignUp(userType);
                    break;
                case 2:
                    await SignIn(userType);
                    break;
                case 3:
                    await ShowMainMenu();
                    break;
                case 4:
                    Console.WriteLine("Exiting Application...");
                    Logger.LogInfo("Application exited by user.");
                    Environment.Exit(0);
                    break;
                default:
                    await InvalidChoice();
                    await ShowSignUpSignInMenu(userType);
                    break;
            }
        }

        private static async Task SignUp(UserRole userType)
        {
            userController?.AddUser(userType);
            Console.WriteLine("Please sign in with your name and password.");
            Console.ReadKey();
            await ShowMainMenu();
        }

        private static async Task SignIn(UserRole userType)
        {
            var (username, validUsername) = ValidationHelper.GetValidUserName("Enter Username: ");
            if (!validUsername) return;

            var (password, validPassword) = ValidationHelper.GetValidUserName("Enter Password: ");
            if (!validPassword) return;

            var (currentUser, isValid) = userController?.ValidateUser(username, password, userType) ?? (null, false);

            if (isValid && currentUser != null)
            {
                await ShowRoleMenu(userType, currentUser);
            }
            else
            {
                Console.WriteLine("Invalid Username or Password. Please try again!");
                Console.ReadKey();
                await ShowSignUpSignInMenu(userType);
            }
        }

        private static async Task ShowRoleMenu(UserRole userType, User currentUser)
        {
            switch (userType)
            {
                case UserRole.Customer:
                    await ShowCustomerActionsMenu(currentUser);
                    break;
                case UserRole.Seller:
                    await ShowSellerMenu(currentUser);
                    break;
                case UserRole.Admin:
                    await ShowAdminMenu(currentUser);
                    break;
            }
        }

        private static async Task ShowAdminMenu(User currentUser)
        {
            if (!ControllersInitialized())
            {
                Logger.LogError("Dependency Injection Failed.");
                return;
            }
            int choice;
            do
            {
                Console.Clear();
                string prompt = """
            ===== Admin Menu =====
            1. Show Products
            2. Show Orders
            3. Show Customers
            4. Show Sellers
            5. Delete User
            6. Logout
            7. Exit Application
            Enter your choice:
            """;
                choice = ValidationHelper.GetValidatedNumberInput(prompt);
                switch (choice)
                {
                    case 1:
                        productController?.ShowAllProducts();
                        await Pause();
                        break;
                    case 2:
                        orderController?.ShowAllOrders();
                        await Pause();
                        break;
                    case 3:
                        userController?.GetAllCustomer(currentUser);
                        await Pause();
                        break;
                    case 4:
                        userController?.GetAllSellers(currentUser);
                        await Pause();
                        break;
                    case 5:
                        userController?.DeleteUser();
                        await Pause();
                        break;
                    case 6:
                        Console.WriteLine("Logging out...");
                        await ShowMainMenu();
                        return;
                    case 7:
                        ExitApplication();
                        return;
                    default:
                        await InvalidChoice();
                        break;
                }

            } while (choice != 7);
        }


        private static async Task ShowCustomerActionsMenu(User currentUser)
        {
            if (!ControllersInitialized())
            {
                Logger.LogError("Dependency Injection Failed.");
                return;
            }
            int choice;
            do
            {
                Console.Clear();
                string prompt = """
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
                choice = ValidationHelper.GetValidatedNumberInput(prompt);
                switch (choice)
                {
                    case 1:
                        productController?.ShowAllProducts();
                        await Pause();
                        break;
                    case 2:
                        productController?.ShowAllProducts();
                        await orderController?.PlaceOrder(currentUser);
                        await Pause();
                        break;
                    case 3:
                        orderController?.ShowCustomerOrders(currentUser);
                        await Pause();
                        break;
                    case 4:
                        userController?.ShowProfile(currentUser);
                        await Pause();
                        break;
                    case 5:
                        await EditProfile(currentUser);
                        await Pause();
                        break;
                    case 6:
                        Console.WriteLine("Logging out...");
                        await ShowMainMenu();
                        return;
                    case 7:
                        ExitApplication();
                        return;
                    default:
                        await InvalidChoice();
                        break;
                }

            } while (choice != 7);
        }


        private static async Task Pause()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
            await Task.CompletedTask;
        }

        private static void ExitApplication()
        {
            Console.WriteLine("Exiting Application...");
            Logger.LogInfo("Application finished successfully.");
            Environment.Exit(0);
        }

        private static async Task EditProfile(User currentUser)
        {
            if (!ControllersInitialized())
            {
                Logger.LogError("Dependency Injection Failed.");
                return;
            }

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
                    await EditUserProfile("Name", ValidationHelper.GetValidUserName("Enter new name: "), currentUser);
                    break;
                case 2:
                    await EditUserProfile("Email", ValidationHelper.GetValidatedEmail("Enter new email: "), currentUser);
                    break;
                case 3:
                    await EditPassword(currentUser);
                    break;
                case 4:
                    await EditUserProfile("Phone", ValidationHelper.GetValidUserName("Enter new Phone Number: "), currentUser);
                    break;
                case 5:
                    return;
                default:
                    await InvalidChoice();
                    break;
            }
        }

        private static async Task EditUserProfile(string field, (string value, bool isValid) input, User currentUser)
        {
            if (input.isValid)
            {
                userController?.EditProfile(currentUser, field, input.value);
                await Pause();
            }
        }

        private static async Task EditPassword(User currentUser)
        {
            (string unm1, bool b1) = ValidationHelper.GetValidUserName("Enter current password: ");
            if (b1 && unm1 == currentUser.Password)
            {
                (string newPassword, bool b5) = ValidationHelper.GetValidatedStringInput("Enter new password: ");
                if (!b5)
                    return;

                    userController?.EditProfile(currentUser, "Password", newPassword);
                    await Pause();
            }
            else
            {
                Console.WriteLine("Incorrect current password. Try again!");
                await Pause();
                await EditProfile(currentUser);
            }
        }

        private static async Task ShowSellerMenu(User currentUser)
        {
            if (!ControllersInitialized())
            {
                Logger.LogError("Dependency Injection Failed.");
                return;
            }
            int choice;
            do
            {
                Console.Clear();
                string prompt = """
            ===== Seller Menu =====
            1. Add Product
            2. Update Product
            3. Update Product Quantity
            4. Show Orders
            5. Show Products
            6. Update Order Status
            7. Delete Product
            8. Show Profile
            9. Edit Profile
            10. Logout
            11. Exit Application
            Enter your choice:
            """;

                choice = ValidationHelper.GetValidatedNumberInput(prompt);
                await HandleSellerChoice(choice, currentUser);
            } while (choice != 11);
        }

        private static async Task HandleSellerChoice(int choice, User currentUser)
        {
            switch (choice)
            {
                case 1:
                    productController?.AddProduct(currentUser); break;

                case 2:
                    productController?.UpdateProduct(currentUser); break;

                case 3:
                    productController?.UpdateProductQuantity(currentUser); break;

                case 4:
                    orderController?.ShowOrdersForSeller(currentUser); break;

                case 5:
                    productController?.ShowSellerProducts(currentUser); break;

                case 6:
                    orderController?.UpdateOrderStatus(currentUser); break;

                case 7:
                    productController?.DeleteProduct(currentUser); break;

                case 8:
                    userController?.ShowProfile(currentUser); break;

                case 9:
                    await EditProfile(currentUser); break;

                case 10:
                    Console.WriteLine("Logging out...");
                    await ShowMainMenu();
                    break;

                case 11:
                    ExitApplication(); break;

                default:
                    await InvalidChoice(); break;
            }

            await Pause();
        }

        private static async Task InvalidChoice()
        {
            Console.WriteLine("Invalid choice. Please try again.");
            await Pause();
        }

    }
}