using System;
using System.Text.RegularExpressions;

namespace ECommerceClassLibrary.Helper
{
    public class ValidationHelper
    {

        public static int GetValidatedNumberInput(string prompt)
        {
            int result;
            string input;
           

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (string.IsNullOrEmpty(input) || !int.TryParse(input, out result) || result < 0)
                {
                    Console.Clear();
                    Console.WriteLine("Invalid input. Please enter a valid number.(non-empty,greater than 0)");
                }

            } while (string.IsNullOrEmpty(input) || !int.TryParse(input, out result) || result < 0);

            return result;
        }


        public static (string,bool) GetValidatedStringInput(string prompt)
        {
           
            string input;
            
            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (input == "break")
                {
                    
                    return (input, false);
                }
                if (string.IsNullOrEmpty(input))
                {
                    
                    Console.WriteLine("Invalid input. Please enter a valid string.\nType 'break' to go back");
                    ;
                }
               

            } while (string.IsNullOrEmpty(input));

            return (input,true);
        }

        public static (string,bool) GetValidatedEmail(string prompt)
        {

            string input;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (input == "break")
                {

                    return (input, false);
                }
                if (input.IsValidEmail())
                {
                    
                    Console.WriteLine("Invalid input. Please enter a valid Email.\nType 'break' to go back");
                }

            } while (input.IsValidEmail());

            return (input, true);
        }

        public static (string, bool) GetValidatedPhone(string prompt)
        {

            string input;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (input == "break")
                {

                    return (input, false);
                }
                if (!input.IsValidPhoneNumber())
                {
                    
                    Console.WriteLine("Invalid input. Please enter a valid Phone Number.\nType 'break' to go back");
                }

            } while (input.IsValidPhoneNumber());

            return (input, true);
        }

        public static (string, bool) IsValidPinCode(string prompt)
        {
            string input;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (input == "break")
                {

                    return (input, false);
                }
                if (string.IsNullOrEmpty(input) || !Regex.IsMatch(input, "^[1-9][0-9]{5}$"))
                {
                    
                    Console.WriteLine("Invalid input. Please enter a valid ZipCode (it should be of 6 digits and does not start with 0 !!).\nType 'break' to go back");
                }

            } while (string.IsNullOrEmpty(input) || !Regex.IsMatch(input, "^[1-9][0-9]{5}$"));

            return (input, true);
        }

        public static (decimal, bool) GetValidatedDecimalInput(string prompt)
        {
            string input;
            decimal result;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (input == "break")
                {

                    return (-1, false);
                }
                if (string.IsNullOrEmpty(input) || !decimal.TryParse(input, out result) || result < 0)
                {
                   
                    Console.WriteLine("Enter Valid Value !!\nType 'break' to go back");
                }

            } while (string.IsNullOrEmpty(input) || !decimal.TryParse(input, out result) || result < 0);

            return (result, true);
        }

        public static (string, bool) GetValidUserName(string prompt)
        {

            string input;

            do
            {
                Console.WriteLine(prompt);
                input = Console.ReadLine();

                if (input == "break")
                {

                    return (input, false);
                }
                if (string.IsNullOrEmpty(input) || !IsValidName(input))
                {
                    Console.WriteLine("Invalid input. Please enter a valid Name.\nType 'break' to go back");
                }

            } while (string.IsNullOrEmpty(input) || !IsValidName(input));

            return (input, true);
        }


        public static  bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }
            string pattern = @"^[A-Za-z][\w@#$%^&*()-]*$";

            if (name.Contains(" ") || !Regex.IsMatch(name, pattern))
            {
                return false;
            }

            return true;
        }

        public static bool IsAlphabetic(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }


    }
}
