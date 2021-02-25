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
                    if (action == "3")
                        Crack();
                }
            }
        }

        private static void Crack()
        {
            // På samma sätt som https://en.wikipedia.org/wiki/List_of_data_breaches
            //DVS Säg att vi fått tag på 
            /*
             *
             *Id	UserId	HashedPassword
            2	stefan	5F4DCC3B5AA765D61D8327DEB882CF99
            3	oliver	6DBD0FE19C9A301C4708287780DF41A2
            4	josefine	C1A93BDFFE7D5062BA3489BBD21E59DC


            //Jaha... men det GÅR ju inte??? 
            //matematiken säger att en hash går inte att "decryptera"
            /och det gör det inte I promise!!
             *
             */

            Console.WriteLine("*** CRACKING DEMO ***");
            Console.Write("Hash:");

            var hash = Console.ReadLine();

            var cracker = new PasswordCracker();
            var clearText = cracker.Solve(hash);

            if (clearText == null)
                Console.WriteLine("Gick inte...");
            else
                Console.WriteLine( $"Cracked. Lösenordet är:{clearText}" );
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
            var hashedPassword = PasswordHasher.CreateHash(password);
            
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
            
            var hashedPassword = PasswordHasher.CreateHash(pwd);

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
            Console.Write("Enter action :>");
            return Console.ReadLine();
        }
    }
}
