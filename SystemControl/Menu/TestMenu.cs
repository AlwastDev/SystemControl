using System;
using System.Collections.Generic;
using System.Linq;
using SystemControl.Models.TestModels;
using SystemControl.Services;

namespace SystemControl.Menu
{
    static class TestMenu
    {

        public static void StartTest(TestModel test)
        {
            Console.Clear();
            Console.WriteLine($"Запустить тест: \"{test.Name}\"");
            for(int i = 0; i < test.QuestionList.Count(); i++)
            {
                var question = test.QuestionList[i];
                Console.WriteLine($"#{i + 1}.\t{question.Question}\n");
                for(int a = 0; a < question.AnswerList.Count(); a++)
                {
                    var answer = question.AnswerList[a];
                    Console.WriteLine($"{a + 1}.\t{answer.Answer}\n");
                }
                Console.Write($"\n\nВаш выбор(1-{question.AnswerList.Count()} across ','): ");
                bool valid = false;
                List<int> choices = new List<int>();
                do
                {
                    string[] tmp = Console.ReadLine().Replace(" ", "").Split(',');
                    foreach(var item in tmp)
                    {
                        if (choices.Count() > question.AnswerList.Count())
                            continue;
                        int choice = -1;
                        valid = int.TryParse(item, out choice);
                        valid = valid && choice > 0 && choice <= question.AnswerList.Count();
                        if (!valid)
                            continue;
                        choices.Add(choice);
                    }
                    break;
                } while (!valid);
                foreach (var item in choices)
                {
                    test.QuestionList[i].AnswerList[item - 1].IsSelect = true;
                }
                Console.WriteLine("\n");
            }
            var res = test.PercentageCorrectAnswers();
            Console.WriteLine($"Ваш результат: {res}%");
            StatisticsService.Add(new Models.StatisticsModel()
            {
                DatePassage = DateTime.Now,
                TestId = test.Id,
                Percent = res,
                UserId = UserService.AuthUser.Id,
                Id = DateTime.Now.Ticks
            });
            Console.WriteLine("Нажмите кнопку чтобы продолжить...");
            Console.ReadKey();
        }

        public static void CreateNewTest()
        {
            TestModel testModel = new TestModel();
            Console.Clear();
            Console.WriteLine("//////////////////Создание тест меню//////////////////");

            Console.Write("Имя: ");
            testModel.Name = Console.ReadLine();
            Console.Write("Категория: ");
            testModel.Category = Console.ReadLine();
            Console.Write("Описание: ");
            testModel.Description = Console.ReadLine();


            string mode = "";
            do
            {
                testModel.QuestionList.Add(createQuestion());
                if (testModel.QuestionList.Count() >= 2)
                {
                    Console.Write("Добавить новый вопрос(Y/n)? ");
                    mode = Console.ReadLine();
                }
            } while (!(mode.ToLower() != "y" && testModel.QuestionList.Count() >= 2));

            testModel.Id = DateTime.Now.Ticks;
            testModel.AuthorId = UserService.AuthUser.Id;
            testModel.CreateDate = DateTime.Now;
            TestService.Create(testModel);
            Console.WriteLine("Нажмите кнопку чтобы продолжить...");
            Console.ReadKey();
        }
        private static QuestionModel createQuestion()
        {
            Console.WriteLine("//////////////////Добавить новый вопрос//////////////////");
            QuestionModel question = new QuestionModel();
            Console.Write("Question: ");
            question.Question = Console.ReadLine();
            question.Id = DateTime.Now.Ticks;
            string mode = "";
            do
            {
                question.AnswerList.Add(createAnswer());
                if(question.AnswerList.Count() >= 2)
                {
                    Console.Write("Добавить новый вопрос(Y/n)? ");
                    mode = Console.ReadLine();
                }
            } while (!(mode.ToLower() != "y" && question.AnswerList.Count() >= 2));

            return question;
        }
        private static AnswerModel createAnswer()
        {
            Console.WriteLine("//////////////////Добавить новый ответ//////////////////");
            AnswerModel answer = new AnswerModel();
            answer.Id = DateTime.Now.Ticks;
            Console.Write("Ответ: ");
            answer.Answer = Console.ReadLine();
            Console.WriteLine("Правильно (Y/n)? ");
            string correctly = Console.ReadLine();
            answer.IsCorrectly = (correctly.ToLower() == "y");
            return answer;
        }

        public static void EditTest()
        {
            Console.Clear();
            var myTestList = TestService.GetMyTests();
            if (myTestList.Count() == 0)
                return;
            ViewTestList(myTestList, "Мои тесты");
            var selectTest = myTestList[ChooseTest(myTestList.Count())];
            string mode = "0";
            do
            {
                Console.WriteLine($"1. Установленое имя \t\t'{selectTest.Name}'");
                Console.WriteLine($"2. Установленое описание \t\t'{selectTest.Description}'");
                Console.WriteLine($"3. Установленая категория \t\t'{selectTest.Category}'");
                Console.WriteLine($"4. Установленые вопросы \t\tКол-во: {selectTest.QuestionList.Count()}");
                Console.WriteLine($"5. Добавить новый вопрос");
                Console.WriteLine($"0. Вернуться назад");
                mode = Console.ReadLine();
                string tmp = "";
                switch (mode)
                {
                    case "1":
                        tmp = selectTest.Name;
                        editStringParameter(ref tmp, "Имя");
                        selectTest.Name = tmp;
                        break;
                    case "2":
                        tmp = selectTest.Description;
                        editStringParameter(ref tmp, "Описание");
                        selectTest.Description = tmp;
                        break;
                    case "3":
                        tmp = selectTest.Category;
                        editStringParameter(ref tmp, "Категория");
                        selectTest.Category = tmp;
                        break;
                    case "4":
                        List<QuestionModel> questions = selectTest.QuestionList;
                        editQuestions(ref questions);
                        selectTest.QuestionList = questions;
                        break;
                    case "5":
                        selectTest.QuestionList.Add(createQuestion());
                        break;
                }
            } while (mode != "0");
            TestService.Edit(selectTest);
        }

        private static void editQuestions(ref List<QuestionModel> questions)
        {
            string mode = "";
            if (questions.Count() == 0)
                return;
            do
            {
                Console.Clear();
                Console.WriteLine("1. Изменение заголовока вопроса");
                Console.WriteLine("2. Изменить ответ");
                Console.WriteLine("3. Удалить вопрос");
                Console.WriteLine("4. Добавить новые ответы");
                Console.WriteLine("0. Вернуться назад");
                mode = Console.ReadLine();
                if(mode == "1" || mode == "2" || mode == "3")
                {
                    Console.Clear();
                    Console.WriteLine("//////////////////Список вопросов//////////////////");
                    for (int i = 0; i < questions.Count(); i++)
                        Console.WriteLine($"#{i + 1}.\t {questions[i].Question}");

                    int selectIndex = ChooseItem(questions.Count(), "вопрос");
                    var selectQuestion = questions[selectIndex];
                    switch (mode)
                    {
                        case "1":
                            var tmp = selectQuestion.Question;
                            editStringParameter(ref tmp, "Вопрос");
                            selectQuestion.Question = tmp;
                            break;
                        case "2":
                            List<AnswerModel> answers = selectQuestion.AnswerList;
                            editAnswer(ref answers);
                            selectQuestion.AnswerList = answers;
                            break;
                        case "3": 
                            questions.Remove(selectQuestion);
                            break;
                        case "4":
                            selectQuestion.AnswerList.Add(createAnswer());
                            break;
                    }
                    if(mode != "3")
                        questions[selectIndex] = selectQuestion;
                }
            } while (mode != "0");
        }

        private static void editAnswer(ref List<AnswerModel> answers)
        {
            string mode = "";
            do
            {
                Console.Clear();
                Console.WriteLine("1. Изменить заголовок ответа");
                Console.WriteLine("2. Изменить правильность");
                Console.WriteLine("3. Удалить ответ");
                Console.WriteLine("0. Вернуться назад");
                mode = Console.ReadLine();
                if (mode == "1" || mode == "2" || mode == "3")
                {
                    Console.Clear();
                    Console.WriteLine("//////////////////Список ответов//////////////////");
                    for (int i = 0; i < answers.Count(); i++)
                        Console.WriteLine($"#{i + 1}.\t {answers[i].Answer}\n\t{answers[i].IsCorrectly}");
                    int selectIndex = ChooseItem(answers.Count(), "ответ");
                    var selectAnswer = answers[selectIndex];
                    switch (mode)
                    {
                        case "1":
                            var tmp = selectAnswer.Answer;
                            editStringParameter(ref tmp, "Ответ");
                            selectAnswer.Answer = tmp;
                            break;
                        case "2":
                            bool isCorrectly = selectAnswer.IsCorrectly;
                            editBoolParameter(ref isCorrectly, "правильно");
                            selectAnswer.IsCorrectly = isCorrectly;
                            break;
                        case "3":
                            answers.Remove(selectAnswer);
                            break;
                    }
                    if (mode != "3")
                        answers[selectIndex] = selectAnswer;
                }
            } while (mode != "0");
        }

        private static void editStringParameter(ref string str, string title)
        {
            Console.Write($"{title} ({str}): ");
            str = Console.ReadLine();
        }
        private static void editBoolParameter(ref bool p, string title)
        {
            Console.Write($"{title} (Y/n)(default - {(p ? 'Y' : 'n')}): ");
            string input = Console.ReadLine().ToLower();
            p = input == "Y" ? true : (input == "n" ? false : p);
        }

        public static void DeleteTest()
        {
            var myTestList = TestService.GetMyTests();
            if (myTestList.Count() == 0)
                return;
            ViewTestList(myTestList, "Мои тесты");
            var selectTest = myTestList[ChooseTest(myTestList.Count())];
            TestService.Delete(selectTest);
        }

        public static void ViewAllTestList()
        {
            Console.WriteLine("//////////////////Список тестов//////////////////");
            var testList = TestService.GetAllTests();
            viewTestList(testList);
        }

        public static void ViewMyTestList()
        {
            ViewTestList(TestService.GetMyTests(), "Мои тесты");
        }
        public static void ViewTestList(List<TestModel> testList, string title)
        {
            Console.WriteLine($"//////////////////{title}//////////////////");
            viewTestList(testList);
        }

        public static int ChooseTest(int limit)
        {
            return ChooseItem(limit, "test");
        }

        public static int ChooseItem(int limit, string itemName)
        {
            int selectIndex = 0;
            bool validInput = false;
            do
            {
                Console.Write($"\n\nПожалуйста выберите {itemName}(1-{limit}): ");
                validInput = int.TryParse(Console.ReadLine(), out selectIndex);
            } while (!validInput || !(selectIndex >= 1 && selectIndex <= limit));
            return selectIndex - 1;
        }

        private static void viewTestList(List<TestModel> testList)
        {
            var users = XmlService.GetUserList();
            for (int i = 0; i < testList.Count(); i++)
            {
                var author = users.FirstOrDefault(p => p.Id == testList[i].AuthorId);
                Console.WriteLine($"#{i + 1}. Имя: {testList[i].Name}\n" +
                    $"\tОписание:\t{testList[i].Description}\n" +
                    $"\tКатегория:\t{testList[i].Category}\n" +
                    $"\tАвтор:\t\t{(author == null ? "NaN" : author.Login)}");
            }
        }
    }
}
