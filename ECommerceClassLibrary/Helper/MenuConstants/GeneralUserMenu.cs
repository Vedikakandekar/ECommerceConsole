using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceClassLibrary.Helper.MenuEnums
{
    public static class UserRoleMenu
    {
        public const int Customer = 1;
        public const int Seller = 2;
        public const int Admin = 3;
        public const int Exit = 4;
    }
    public static class UserActionMenu
    {
        public const int SignUp = 1;
        public const int SignIn = 2;
        public const int BackToMainMenu = 3;
        public const int Exit = 4;
    }
    public static class ProfileEditMenuOptions
    {
        public const int EditName = 1;
        public const int EditEmail = 2;
        public const int EditPassword = 3;
        public const int EditPhoneNumber = 4;
        public const int Back = 5;
    }


}
