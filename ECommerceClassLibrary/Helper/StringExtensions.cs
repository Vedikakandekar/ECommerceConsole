using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerceClassLibrary.Helper
{
    public static class StringExtensions
    {
        public static bool IsValidEmail(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        public static bool IsValidPhoneNumber(this string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return false;
            }

            string phonePattern = @"^[7-9]{10}$";

            return Regex.IsMatch(phoneNumber, phonePattern);
        }
    }
}
