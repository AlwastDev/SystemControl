using SystemControl.Models;
using SystemControl.Services;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading;

namespace SystemControl.Menu
{
    static class UserMenu
    {
        public static void Registration()
        {
            Console.WriteLine("//////////////////Регистрация///////////////////");
            UserModel userModel = new UserModel();
            Console.Write("Твое имя: ");
            userModel.Name = Console.ReadLine();
            do
            {
                Console.Write("Логин: ");
                userModel.Login = Console.ReadLine();
                if (!UserService.LoginCheckUniqueness(userModel.Login))
                    Console.WriteLine("Логин должен быть уникальным!");
            } while (!UserService.LoginCheckUniqueness(userModel.Login));
            Console.Write("Пароль: ");
            Console.ForegroundColor = ConsoleColor.Black;
            userModel.Password = Console.ReadLine();

            Console.ResetColor();
            Console.Clear();
            try
            {
                UserService.Registration(userModel);
                Console.WriteLine($"Вы успешно зарегистрировались");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Нажмите кнопку чтобы продолжить...");
            Console.ReadKey();
        }

        public static void Authentication()
        {
            Console.WriteLine("//////////////////Вход в Уч. Запись//////////////////");
            UserModel userModel = new UserModel();
            Console.Write("Логин: ");
            userModel.Login = Console.ReadLine();
            Console.Write("Пароль: ");
            Console.ForegroundColor = ConsoleColor.Black;
            userModel.Password = Console.ReadLine();
            Console.ResetColor();
            try
            {
                UserService.Authentication(userModel);
                Console.WriteLine($"Приветствую {UserService.AuthUser.Name}!");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Нажмите кнопку чтобы продолжить...");
            Console.ReadKey();
        }

        public static void ViewMyStats()
        {
            var myStats = XmlService.GetStatistics().Where(p => p.UserId == UserService.AuthUser.Id);
            var testList = XmlService.GetTestList();
            Console.Clear();
            Console.WriteLine("//////////////////Моя статистика//////////////////");
            foreach(var item in myStats)
            {
                var test = testList.FirstOrDefault(p => p.Id == item.TestId);
                Console.WriteLine($"Дата:\t{item.DatePassage}\nТестовое имя:\t{test.Name}\nТест категории:\t{test.Category}\nПроцент:\t{item.Percent}%");
                Console.WriteLine("----------------------------");
            }
            Console.WriteLine("\n\nНажмите кнопку чтобы продолжить...");
            Console.ReadKey();
        }

        public static void Settings()
        {
            string mode = "";
            do
            {
                Console.Clear();
                Console.WriteLine("1. Установить пароль");
                Console.WriteLine("2. Сменить пароль");
                Console.WriteLine("0. Вернуться назад");
                mode = Console.ReadLine();
                setPassword();
            } while (mode != "0");
        }

        private static void setPassword()
        {
            bool isViewError = false;
            string password = "";
            while(true)
            {
                Console.Clear();
                if (isViewError)
                    Console.WriteLine("Неправильный пароль!\n");
                Console.Write("Старый пароль: ");
                Console.ForegroundColor = ConsoleColor.Black;
                password = Console.ReadLine();
                Console.ResetColor();
                isViewError = !UserService.CkeckPassword(password);
                if (!isViewError)
                    break;
            }
            Console.Write("Новый пароль: ");
            Console.ForegroundColor = ConsoleColor.Black;
            password = Console.ReadLine();
            Console.ResetColor();
            UserService.SetPassword(password);
            Console.WriteLine("Пароль успешно изменен!");
            Thread.Sleep(1250);
        }
    }
}
