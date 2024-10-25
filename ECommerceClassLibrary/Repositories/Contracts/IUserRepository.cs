using ECommerceClassLibrary.Models;
using System.Collections.Generic;

namespace ECommerceClassLibrary.Repositories.Contracts
{
    public interface IUserRepository
    {
        User GetUserById(int id);

        (User, bool) ValidateUser(string unm, string pass, UserRole userType);
        void CustomerSignUp(User user);
        void ShowProfile(User currentCustomer);
        void EditProfile(User currentCustomer, string feild, dynamic newValue);

        int GetUserCount();

        bool IsUserNameUnique(string userName);

        bool DeleteUser(User user);

        List<Customer> GetAllCustomers();

        List<Seller> GetAllSellers();

        List<User> GetAllUsers();
    }
}
