namespace ECommerceClassLibrary.Repositories
{
    public abstract class User
    {
        private int userId;

        private string name;

        private string email;

        private string password;
        public string phoneNumber { get; set; }

        private UserRole role;

        public string Name
        {
            get => name;
            set => name = value;
        }

        public int UserId
        {
            get => userId;
            set => userId = value;
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set => phoneNumber = value;
        }

        public string Email
        {
            get => email;
            set => email = value;
        }

        public string Password
        {
            get => password;
            set => password = value;
        }

        public UserRole Role
        {
            get => role;
            set => role = value;
        }

    }

   public enum UserRole {
        Admin = 1,
        Customer = 2,
        Seller = 3,
    }
}
