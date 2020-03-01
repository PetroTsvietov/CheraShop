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
            DataBase db = new DataBase();
            Account ac = new Account();
            db.createFiles();
            ac.enterToDB();
            ac.autorization();
            Console.ReadKey();
        }
    }
    class StartMenu
    {
        Account ac = new Account();
        public void start()
        {
            Console.WriteLine("Ebat ti krasavela");
        }
    }
    struct Users
    {
        public string login;
        public string password;
    }
    class DataBase
    {

        public void textFiles(string path, string elements) // метод создания файла
        {
            using (FileStream fs = File.Create(path)) {
                byte[] info = new UTF8Encoding(true).GetBytes(elements);
                fs.Write(info, 0, info.Length);
            }
        }
        public void createFiles() //ну типа создаю файлики собсна
        {
            string fruitPath = @"D:\fruit.txt";
            string fruitElements = "1.apple\n2.pineapple\n3.banana";
            string vegPath = @"D:\veg.txt";
            string vegElements = "1.Vladlen\n2.Chera\n3.Mama";
            string cherPath = @"D:\cherSamotiki.txt";
            string cherElements = "1.Dominator\n2.Big Boy\n3.King Kong";
            string DBPath = @"D:\DB.txt";
            string DBElements = "Admin\nPeter";
            textFiles(fruitPath, fruitElements);
            textFiles(vegPath, vegElements);
            textFiles(cherPath, cherElements);
            textFiles(DBPath, DBElements);
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
        StartMenu sm = new StartMenu();
        public List<ModelAcc> user = new List<ModelAcc>();
        Users users;
        Validation val = new Validation();
        public void enterToDB()
        {
            string path = @"D:\DB.txt";
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

        public void registration()
        {
            Console.WriteLine("Hello! Start registration please!\nEnter your login:");
            string new_login = Console.ReadLine();
            if (val.regist(new_login) == false)
            {
                Console.WriteLine("This login is not avilable");
                registration();
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
                    registration();
                }
                else
                {
                    Console.WriteLine("Congratulations!");
                    string writePath = @"D:\DB.txt";

                    using (StreamWriter sw = new StreamWriter(writePath, true, System.Text.Encoding.Default))
                    {
                        Console.WriteLine("\n");
                        sw.WriteLine(new_login);
                        sw.WriteLine(new_password);
                    }
                    enterToDB();
                }
            }
        }//регистрация
        public void autorization()
        {
            Console.WriteLine("Hello! If you want create account enter 1, if you want autorization enter 2");
            string choice = Console.ReadLine();
            if (choice == "1")
            {
                registration();
            }
            else if (choice == "2")
            {
                Console.Write("Login:");
                string login2 = Console.ReadLine();
                Console.Write("Password:");
                string password2 = Console.ReadLine();
                foreach (ModelAcc u in user)
                {
                    if (val.Equals(new ModelAcc(login2, password2)) == true)
                    {
                        sm.start();
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Your password or login is invalid! Or you invalid");
                        autorization();
                    }
                }
            }
            else{
                autorization();
            }
        }//авторизация
    }
    class Validation
    {
        Account ac = new Account();
        public override bool Equals(object o)
        {
            Account A = new Account();
            ac.enterToDB();
            ModelAcc currentUser = o as ModelAcc;
            if ((currentUser) != null)
            {
                foreach (ModelAcc u in A.user)
                {
                    if (u.login == currentUser.login && u.password == currentUser.password)
                    {
                        return true;
                    }
                }
            }
            return false;
        }//проверка на наличие аккаунта в БД
        public bool regist(string trueLogin)
        {
            string path = @"D:\DB.txt";
            using (StreamReader sr = new StreamReader(path, Encoding.Default))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (trueLogin != line)
                    {
                        return true;
                    }
                }
            }
            return false;
        }//проверка на логин
    }
}
