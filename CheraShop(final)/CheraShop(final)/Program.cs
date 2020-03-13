using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cheraShop5
{
    class Program
    {
        static void Main(string[] args)
        {
            DataBase database = new DataBase();
            Account account = new Account();
            database.TextFiles();
            account.EnterToDB();
            account.Registration();
            Console.ReadKey();
        }
    }
    class StartMenu
    {
        public void EnterToBasket(string path, int numberChoice)//метод заносит ебучие продукты в ебучую корзину нахуй
        {
            string pathBasket = @"basket.txt";
            Console.WriteLine("Enter number of product and is will spend to your basket");
            numberChoice = Convert.ToInt32(Console.ReadLine());
            string[] numberLine = File.ReadAllLines(path);
            using(StreamWriter sw = new StreamWriter(pathBasket, true, Encoding.Default))
            {
                Console.WriteLine(numberLine[numberChoice]);
            }
            OutText(path);
        }
        public void OutText(string pathOutText) //метод выводит список продуктов(он у меня в кейсах там юзается) и работает дальше
        {
            using (StreamReader sr = new StreamReader(pathOutText))
            {
                Console.WriteLine(sr.ReadToEnd());
                int choice = Convert.ToInt32(Console.ReadLine());
                EnterToBasket(pathOutText, choice);
            }
            
        }
        public void Start()
        {
            Account Account = new Account();
            Console.WriteLine("Hello, enter a catalog, what you need.\n1.Fruits\n2.Vegetables\n3.Faloses\n4.Basket\n5.Exit from account");
            string numberOfCatalog = Console.ReadLine();
            switch (numberOfCatalog)
            {
                case "1":
                    OutText(@"fruit.txt");
                    break;
                case "2":
                    OutText(@"veg.txt");
                    break;
                case "3":
                    OutText(@"cherSamotiki.txt");
                    break;
                case "4":
                    OutText(@"basket.txt");
                    break;
                case "5":
                    Account.Registration();
                    break;
            }

        }
    }
    struct Users
    {
        public string login;
        public string password;
    }
    class DataBase
    {
        public void TextFiles() // метод создания файла
        {
            string[] path = new string[] { @"fruit.txt", @"veg.txt", @"cherSamotiki.txt", @"DB.txt", @"basket.txt" };
            string[] elements = new string[] { "1.apple\n2.pineapple\n3.banana\n4.Back", "1.Vladlen\n2.Chera\n3.Mama\n4.Back", "1.Dominator\n2.Big Boy\n3.King Kong\n4.Back", "Peter\nAdmin", "" };
            for (int i = 0; i < path.Length; i++)
            {
                File.AppendAllLines(path[i], new string[] { elements[i] });
            }
        }
    }
    class ModelAcc
    {
        public string login;
        public string password;
        public ModelAcc(string login, string password) //модель аккаунта для валидации при авторизации, регистрации и заноса в бд
        {
            this.login = login;
            this.password = password;
        }
    }
    class Account
    {
        public List<ModelAcc> user = new List<ModelAcc>();
        Users users;
        public void EnterToDB()
        {
            string path = @"DB.txt";
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                string line;
                int i = 0;
                while ((line = sr.ReadLine()) != null)
                {
                    i++;
                    if (i % 2 != 0)
                    {
                        users.login = line;
                    }
                    else if (i % 2 == 0)
                    {
                        users.password = line;
                    }
                    if (!string.IsNullOrEmpty(users.login) && !string.IsNullOrEmpty(users.password))
                    {
                        user.Add(new ModelAcc(users.login, users.password));
                        users.login = null;
                        users.password = null;
                    }
                }
            }
            Console.WriteLine(user.Count);
        } //добавляем в список аккаунты с БД
        public void Registration()
        {
            Validation validation = new Validation();
            Console.WriteLine("Enter 1 if you want create account or somthing else if you want do autorization");
            string regOrAuth = Console.ReadLine();
            if (regOrAuth != "1")
            {
                Autorization();
            }
            else
            {
                Console.WriteLine("Hello! Start registration please!\nEnter your login:");
                string new_login = Console.ReadLine();
                if (validation.Regist(new_login) == false)
                {
                    Console.WriteLine("This login is not avilable");
                    Registration();
                }
                else
                {
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
                        string writePath = @"DB.txt";

                        using (StreamWriter sw = new StreamWriter(writePath, true, Encoding.Default))
                        {
                            Console.WriteLine("\n");
                            sw.WriteLine(new_login);
                            sw.WriteLine(new_password);
                        }
                        EnterToDB();
                        Autorization();
                    }
                }
            }
        }//регистрация
        public void Autorization()
        {
            StartMenu startmenu = new StartMenu();
            Validation validation = new Validation();
            Console.Write("Login:");
            string login2 = Console.ReadLine();
            Console.Write("Password:");
            string password2 = Console.ReadLine();
            foreach (ModelAcc u in user)
            {
                if (validation.Equals(new ModelAcc(login2, password2)) == true)
                {
                    startmenu.Start();
                    break;
                }
                else
                {
                    Console.WriteLine("Your password or login is invalid! Or you invalid");
                    Autorization();
                }
            }
        }//авторизация
    }
    class Validation
    {
        public override bool Equals(object o)
        {
            Account account = new Account();
            account.EnterToDB();
            ModelAcc currentUser = o as ModelAcc;
            if ((currentUser) != null)
            {
                foreach (ModelAcc u in account.user)
                {
                    if (u.login == currentUser.login && u.password == currentUser.password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }//проверка на наличие аккаунта в БД
        public bool Regist(string trueLogin)
        {
            string path = @"DB.txt";
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (trueLogin == line)
                    {
                        return false;
                    }
                }
            }
            return true;
        }//проверка на логин
    }
}
