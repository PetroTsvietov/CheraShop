using System;
using System.Collections.Generic;

namespace chera_shop_2
{
    class Program
    {
        static void Main(string[] args)
        {
            User u = new User("login", "password");
            List<User> users = new List<User>();
            Account A = new Account();
            A.Registration();
            StarMenu SM = new StarMenu();
            SM.start();
            Console.WriteLine(users.Count);
            Console.ReadKey();
        }
    }
    class StarMenu
    {
        public void start()
        {
            Console.WriteLine("Hello! Enter catalog that interested you");
        }
    }

    class User
    { 
        public string login;
        public string password;
        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }

    class Account
    {
        User u = new User("login", "password");
        List<User> users = new List<User>();
        public void Registration()
        {
            Console.WriteLine("Enter your login:");
            string new_login = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string new_password = Console.ReadLine();
            Console.WriteLine("Re-enter password:");
            string re_new_password = Console.ReadLine();
            if(re_new_password != new_password)
            {
                Console.WriteLine("You entered the wrong password again");
                Registration();
            }
            else
            {
                Console.WriteLine("Congratulations!");
                users.Add(new User(new_login, new_password));
                Autorization();
            }
        }
        public void Autorization()
        {

        }
    }
}
