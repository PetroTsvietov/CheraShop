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
        public void EnterToBasket(string path, int numberChoice)//метод заносит продукты в корзину
        {
            string pathBasket = @"basket.txt";
            Console.WriteLine("Enter number of product and is will spend to your basket");
            numberChoice = Convert.ToInt32(Console.ReadLine());
            string[] numberLine = File.ReadAllLines(path);
            using (StreamWriter sw = new StreamWriter(pathBasket, true, Encoding.Default))
            {
                if(numberChoice == 1)
                {
                    sw.WriteLine(numberLine[0], numberLine[1]);
                }else if(numberChoice == 2)
                {
                    sw.WriteLine(numberLine[2], numberLine[3]);
                }else if(numberChoice == 3)
                {
                    sw.WriteLine(numberLine[4], numberLine[5]);
                }else if(numberChoice == 4)
                {
                    Start();
                }
                
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

        } //метод вывода каталогов
    }
    struct Users
    {
        public string login;
        public string password;
        private double _balance;
        public double balance
        {
            get
            {
                return _balance;
            }
            set
            {
                if (_balance < 0)
                {
                    throw new Exception();
                }
                _balance = value;
            }
        }
    }
    class DataBase
    {
        public void TextFiles() // метод создания файла
        {
            string[] path = new string[] { @"fruit.txt", @"veg.txt", @"cherSamotiki.txt", @"DB.txt", @"basket.txt" };
            string[] elements = new string[] { "1.apple\n10\n2.pineapple\n15\n3.banana\n11\n4.Back", "1.Vladlen\n0\n2.Chera\n1000\n3.Mama\n1\n4.Back", "1.Dominator\n80\n2.Big Boy\n100\n3.King Kong\n500\n4.Back", "Peter\nAdmin\n10000", "" };
            for (int i = 0; i < path.Length; i++)
            {
                if (File.Exists(path[i]))
                {
                    break;
                }
                else
                {
                    File.AppendAllLines(path[i], new string[] { elements[i] });
                }
            }
        }
    }
    class ModelAcc
    {
        public string login;
        public string password;
        private double _balance;
        public double balance
        {
            get
            {
                return _balance;
            }
            set
            {
                if (_balance < 0)
                {
                    throw new Exception();
                }
                _balance = value;
            }
        }
        public ModelAcc(string login, string password, double balance) //модель аккаунта для валидации при авторизации, регистрации и заноса в бд
        {
            this.login = login;
            this.password = password;
            this.balance = balance;
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
                string[] FileLines = File.ReadAllLines(path);
                    for (int i = 0; i < FileLines.Length; i += 3)
                    {
                        users.login = FileLines[i];
                        users.password = FileLines[i + 1];
                        users.balance = Convert.ToInt64(FileLines[i + 2]);
                        if (!string.IsNullOrEmpty(users.login) && !string.IsNullOrEmpty(users.password))
                        {
                            user.Add(new ModelAcc(users.login, users.password, users.balance));
                            users.login = null;
                            users.password = null;
                        }
                    }
            }
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
                        Random rnd = new Random();
                        users.balance = rnd.Next(100, 10000);
                        Console.WriteLine("Congratulations! Your balance is " + users.balance);
                        string writePath = @"DB.txt";

                        using (StreamWriter sw = new StreamWriter(writePath, true, Encoding.Default))
                        {
                            Console.WriteLine("\n");
                            sw.WriteLine(new_login);
                            sw.WriteLine(new_password);
                            sw.WriteLine(users.balance);
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
                if (validation.Equals(new ModelAcc(login2, password2, u.balance)) == true)
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
