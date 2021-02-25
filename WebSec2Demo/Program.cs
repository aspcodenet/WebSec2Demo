using System;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WebSec2Demo.Data;

namespace WebSec2Demo
{
    //Testa att regga tre konton
    //          stefan, password
    //          oliver, qwerty1
    //          josefine, nd77aS?+13;RR432

    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new DatabaseContext())
            {
                context.Database.Migrate();

                while (true)
                {
                    string action = ShowMenu();
                    if (action == "1")
                        Register(context);
                    if (action == "2")
                        Login(context);
                }
            }
        }

        private static void Register(DatabaseContext context)
        {
            Console.WriteLine("*** REGISTRERA NY ANV ***");
            string username;
            Console.Write("Username:");
            username = Console.ReadLine();
            if (context.Accounts.Any(r => r.UserId == username))
            {
                Console.WriteLine("Du är redan registrerad hos oss?");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
                return;
            }

            Console.Write("Password:");
            var password = Console.ReadLine();
            var hashedPassword = CreateHash(password);
            
            var user = new UserAccount { UserId = username, HashedPassword = hashedPassword};
            context.Accounts.Add(user);
            context.SaveChanges();
        }

        private static void Login(DatabaseContext context)
        {
            Console.WriteLine("***** login ******");
            Console.Write("Enter username:");
            string uid = Console.ReadLine();
            Console.Write("Enter password:");
            string pwd = Console.ReadLine();
            var account = context.Accounts.FirstOrDefault(a => a.UserId == uid);
            
            var hashedPassword = CreateHash(pwd);

            if (account == null || account.HashedPassword != hashedPassword)
            {
                Console.WriteLine("Invalid username or password");
                Console.WriteLine("Press enter to continue");
                Console.ReadLine();
                return;
            }
            Console.WriteLine($"You are logged in as {account.UserId}");
            Console.WriteLine("Press enter to logout");
            Console.ReadLine();
        }



        private static string ShowMenu()
        {
            Console.WriteLine("1. Register as new user");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Crack a hash");
            return Console.ReadLine();
        }



        public static string CreateHash(string input)
        {
            // Use input string to calculate MD5 hash
            StringBuilder sb = new StringBuilder();
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }

    }
}
