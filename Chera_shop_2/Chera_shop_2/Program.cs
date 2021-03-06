﻿using System;
using System.Collections.Generic;
using System.IO;

namespace chera_shop_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Account A = new Account();
            A.DataBase();
            A.Registration();
            StarMenu SM = new StarMenu();
            SM.start();
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
    struct Use
    {
        public string login;
        public string password;
    }
    class Account
    {
        StarMenu sm = new StarMenu();
        User u = new User("login", "password");
       public List<User> users = new List<User>();
        Validation val = new Validation();
        Use use;
        public void DataBase()
        {
            string path = @"D:\HLAM\DB.txt";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    i++;
                    if (i % 2 != 0)
                    {
                        use.login = line;
                    }
                    else if (i % 2 == 0)
                    {
                        use.password = line;
                    }
                    if (!string.IsNullOrEmpty(use.login) && !string.IsNullOrEmpty(use.password))
                    {
                        users.Add(new User(use.login, use.password));
                        use.login = null;
                        use.password = null;
                    }
                }
            }
            Console.WriteLine(users.Count);
        }
        public void Registration()
        {
            Console.WriteLine("Enter your login:");
            string new_login = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string new_password = Console.ReadLine();
            Console.WriteLine("Re-enter password:");
            string re_new_password = Console.ReadLine();
            if (re_new_password != new_password)
            {
                Console.WriteLine("You entered the wrong password again");
                Registration();
            }
            else
            {
                Console.WriteLine("Congratulations!");
                string writePath = @"D:\HLAM\DB.txt";

                using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                {
                    Console.WriteLine("\n");
                    sw.WriteLine(new_login);
                    sw.WriteLine(new_password);
                }
                DataBase();
                Autorization();
            }
        }
        public void Autorization()
        {
            Console.Write("Login:");
            string login2 = Console.ReadLine();
            Console.Write("Password:");
            string password2 = Console.ReadLine();
            foreach (User u in users)
            {
                if (u.Equals(new User(login2, password2)))
                {
                    sm.start();
                }
                else
                {
                    Console.WriteLine("Your password or login is invalid! Or you invalid");
                    Autorization();
                }
            }
        }
    }
    class Validation
    {
            public override bool Equals(object o) {
            Account A = new Account();
            User currentUser = o as User;
            if ((currentUser)!=null)
            {
                foreach (User u in A.users)
                {
                    if (u.login == currentUser.login && u.password == currentUser.password)
                    {
                        return true;
                    }
                }
            }
                return false;
            }
        }
    }
