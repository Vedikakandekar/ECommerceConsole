using ECommerceClassLibrary.Repositories;

namespace ECommerceClassLibrary.Services.Contracts
{
    public interface IUserService
    {
        User GetUserById(int id);

        (User, bool) ValidateUser(string nm, string pass, string fromController);

        void SignUp(string userType);
        void ShowProfile(User currentCustomer);
        void EditProfile(User currentCustomer, string feild, dynamic newValue);

        void GetAllCustomers();
        void GetAllSellers();

        void GetAllUsers();

        void DeleteUser(User user);

    }
}
