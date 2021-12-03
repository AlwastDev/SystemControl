using SystemControl.Models.TestModels;
using SystemControl.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SystemControl.Menu
{
    class MainMenu
    {
        public void StartMenu()
        {
            registrationOrAuthentication();
            mainMenu();
        }

        private void registrationOrAuthentication()
        {
            do
            {
                string selectMode = "0";
                do
                {
                    Console.Clear();
                    Console.WriteLine("1. Регистрация");
                    Console.WriteLine("2. Авторизация");
                    selectMode = Console.ReadLine();
                } while (!(selectMode == "1" || selectMode == "2"));
                Console.Clear();
                switch (selectMode)
                {
                    case "1":
                        UserMenu.Registration();
                        break;
                    case "2":
                        UserMenu.Authentication();
                        break;
                }
            } while (UserService.AuthUser == null);
        }

        private void mainMenu()
        {
            int mode = -1;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Список всех тестов");
                Console.WriteLine("2. Список моих тестов");
                Console.WriteLine("3. Список тестов категории");
                Console.WriteLine("4. Создать новый тест");
                Console.WriteLine("5. Мои статистика");
                Console.WriteLine("6. Пользовательские настройки");
                Console.WriteLine("0. Выход");
                Console.Write("Твой выбор(0-6): ");
                if (!int.TryParse(Console.ReadLine(), out mode))
                    continue;
                Console.Clear();
                switch (mode)
                {
                    case 1: viewListAllTests(); break;
                    case 2: viewListMyTests(); break;
                    case 3: viewTestsInCategory(); break;
                    case 4: TestMenu.CreateNewTest(); break;
                    case 5: UserMenu.ViewMyStats(); break;
                    case 6: UserMenu.Settings(); break;
                    case 0: Environment.Exit(0); break;
                }
            } while (true);
        }


        private void viewTestsInCategory()
        {
            bool isExistCategory = false;
            string category = "";
            string mode = "";
            var testsList = TestService.GetAllTests();
            if (testsList.Count() == 0)
            {
                Console.WriteLine("\n\n\nСписок пустой!");
                Thread.Sleep(1500);
                return;
            }
            List<TestModel> viewTests = new List<TestModel>();
            do
            {
                Console.Clear();
                Console.Write("Введите имя категории: ");
                category = Console.ReadLine();
                viewTests = testsList.Where(p => p.Category == category).ToList();
                if (viewTests.Count() == 0)
                {
                    Console.WriteLine($"Нет тестов в \"{category}\" категории!");
                    Console.WriteLine("\n1. Новый запрос");
                    Console.WriteLine("0. Вернуться");
                    mode = Console.ReadLine();
                    if (mode != "1")
                        return;
                    isExistCategory = false;
                }
                else
                {
                    isExistCategory = true;
                }
            } while (!isExistCategory);

            do
            {
                Console.Clear();
                TestMenu.ViewTestList(viewTests, "Список тестов");
                Console.WriteLine("\n\n1. Выберите тест, который хотите пройти...");
                Console.WriteLine("0. Вернуться");
                Console.Write("Пожалуйста, выберите режим: ");
                mode = Console.ReadLine();
                if (mode == "1" && viewTests.Count() > 0)
                    TestMenu.StartTest(viewTests[TestMenu.ChooseTest(viewTests.Count())]);
            } while (mode != "0");
        }

        private void viewListAllTests()
        {
            string mode = "";
            var testList = TestService.GetAllTests();
            do
            {
                Console.Clear();
                TestMenu.ViewTestList(testList, "Список тестов");
                Console.WriteLine("\n\n1. Выберите тест, который хотите пройти...");
                Console.WriteLine("0. Вернуться");
                Console.Write("Пожалуйста, выберите режим: ");
                mode = Console.ReadLine();
                if (mode == "1" && testList.Count() > 0)
                    TestMenu.StartTest(testList[TestMenu.ChooseTest(testList.Count())]);
            } while (mode != "0");
        }

        private void viewListMyTests()
        {
            string mode = "";
            do
            {
                Console.Clear();
                Console.WriteLine("1. Посмотреть список");
                Console.WriteLine("2. Запустить тест");
                Console.WriteLine("3. Изменить тест");
                Console.WriteLine("4. Удалить тест");
                Console.WriteLine("0. Вернуться");
                Console.Write("Пожалуйста, выберите режим: ");
                mode = Console.ReadLine();
                Console.Clear();
                switch (mode)
                {
                    case "1":
                        TestMenu.ViewMyTestList();
                        Console.WriteLine("Нажмите клавишу чтобы продолжить...");
                        Console.ReadKey();
                        break;
                    case "2": 
                        var testList = TestService.GetMyTests();
                        if (testList.Count() == 0)
                            break;
                        TestMenu.ViewTestList(testList, "Мои тесты");
                        TestMenu.StartTest(testList[TestMenu.ChooseTest(testList.Count())]);
                        break;
                    case "3": TestMenu.EditTest(); break;
                    case "4": TestMenu.DeleteTest(); break;
                }
            } while (mode != "0");
        }

    }
}
