using SystemControl.Models;
using SystemControl.Models.TestModels;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Linq;

namespace SystemControl.Services
{
    static class XmlService
    {

        public static List<UserModel> GetUserList()
        {
            List<UserModel> users = new List<UserModel>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Configurations.UserXML);
            XmlElement xRoot = xDoc.DocumentElement;

            foreach (XmlNode xnode in xRoot)
            {
                UserModel user = new UserModel();
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null)
                    {
                        user.Id = long.Parse(attr.Value);
                    }
                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    switch (childnode.Name)
                    {
                        case "name":
                            user.Name = childnode.InnerText;
                            break;
                        case "login":
                            user.Login = childnode.InnerText; 
                            break;
                        case "password":
                            user.Password = childnode.InnerText;
                            break;
                    }
                }
                users.Add(user);
            }
            return users;
        }
        public static void AddUser(UserModel user)
        {
            XDocument xdoc = XDocument.Load(Configurations.UserXML);
            XElement root = xdoc.Element("users");
            root.Add(new XElement("user",
                        new XAttribute("id", user.Id),
                        new XElement("name", user.Name),
                        new XElement("login", user.Login),
                        new XElement("password", user.Password)));
            File.Delete(Configurations.UserXML);
            xdoc.Save(Configurations.UserXML);
        }
        public static void SetUser(UserModel user)
        {
            XDocument xdoc = XDocument.Load(Configurations.UserXML);
            XElement root = xdoc.Element("users");
            var xUser = root.Elements("user").ToList().FirstOrDefault(p => p.Attribute("id").Value == user.Id.ToString());
        
            if(xUser == null)
                throw new Exception("User not found!");

            xUser.Element("login").Value = user.Login;
            xUser.Element("name").Value = user.Name;
            xUser.Element("password").Value = user.Password;
            File.Delete(Configurations.UserXML);
            xdoc.Save(Configurations.UserXML);
        }
        public static void DeleteUser(UserModel user)
        {
            XDocument xdoc = XDocument.Load(Configurations.UserXML);
            XElement root = xdoc.Element("users");

            var foundUser = root.Elements("user").ToList().FirstOrDefault(p => p.Attribute("id").Value == user.Id.ToString());
            foundUser.Remove();

            File.Delete(Configurations.UserXML);
            xdoc.Save(Configurations.UserXML);
        }

        public static List<StatisticsModel> GetStatistics()
        {
            List<StatisticsModel> statisticsList = new List<StatisticsModel>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Configurations.StatisticsXML);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                StatisticsModel statistics = new StatisticsModel();
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null)
                    {
                        statistics.Id = long.Parse(attr.Value);
                    }
                }
                foreach (XmlNode childnode in xnode.ChildNodes)
                {
                    switch (childnode.Name)
                    {
                        case "userId":
                            statistics.UserId = long.Parse(childnode.InnerText);
                            break;
                        case "testId":
                            statistics.TestId = long.Parse(childnode.InnerText);
                            break;
                        case "percent":
                            statistics.Percent = double.Parse(childnode.InnerText);
                            break;
                        case "datePassage":
                            statistics.DatePassage = DateTime.Parse(childnode.InnerText);
                            break;
                    }
                }
                statisticsList.Add(statistics);
            }
            return statisticsList;
        }
        public static void AddStatistics(StatisticsModel statistics)
        {
            XDocument xdoc = XDocument.Load(Configurations.StatisticsXML);
            XElement root = xdoc.Element("statistics");
            root.Add(new XElement("statisticsObj",
                                        new XAttribute("id", statistics.Id),
                                        new XElement("userId", statistics.UserId),
                                        new XElement("testId", statistics.TestId),
                                        new XElement("percent", statistics.Percent),
                                        new XElement("datePassage", statistics.DatePassage)));
            File.Delete(Configurations.StatisticsXML);
            xdoc.Save(Configurations.StatisticsXML);
        }
        public static void DeleteStatistics(StatisticsModel statistics)
        {
            XDocument xdoc = XDocument.Load(Configurations.StatisticsXML);
            XElement root = xdoc.Element("statistics");
            var foundStatistics = root.Elements("statisticsObj").ToList().FirstOrDefault(p => p.Attribute("id").Value == statistics.Id.ToString());
            foundStatistics.Remove();

            File.Delete(Configurations.StatisticsXML);
            xdoc.Save(Configurations.StatisticsXML);
        }

        public static List<TestModel> GetTestList()
        {
            List<TestModel> testList = new List<TestModel>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Configurations.TestXML);
            XmlElement xRoot = xDoc.DocumentElement;
            foreach (XmlNode xnode in xRoot)
            {
                TestModel testModel = new TestModel();
                if (xnode.Attributes.Count > 0)
                {
                    XmlNode attr = xnode.Attributes.GetNamedItem("id");
                    if (attr != null)
                        testModel.Id = long.Parse(attr.Value);
                }
                foreach (XmlNode node in xnode.ChildNodes)
                {
                    switch (node.Name)
                    {
                        case "name":
                            testModel.Name = node.InnerText;
                            break;
                        case "authorId":
                            testModel.AuthorId = long.Parse(node.InnerText);
                            break;
                        case "category":
                            testModel.Category = node.InnerText;
                            break;
                        case "createDate":
                            testModel.CreateDate = DateTime.Parse(node.InnerText);
                            break;
                        case "description":
                            testModel.Description = node.InnerText;
                            break;
                    }
                    if (node.Name == "questionList")
                        testModel.QuestionList = parseQuestions(node.ChildNodes);
                }
                testList.Add(testModel);
            }
            return testList;
        }
        public static void AddTest(TestModel test)
        {
            XDocument xdoc = XDocument.Load(Configurations.TestXML);
            XElement root = xdoc.Element("tests");
            XElement xTest = new XElement("test",
                                        new XAttribute("id", test.Id),
                                        new XElement("name", test.Name),
                                        new XElement("authorId", test.AuthorId),
                                        new XElement("category", test.Category),
                                        new XElement("createDate", test.CreateDate),
                                        new XElement("description", test.Description));
            XElement xQuestionList = new XElement("questionList");
            foreach(var question in test.QuestionList)
            {
                var xQuestionRoot = new XElement("questionObj",
                                    new XAttribute("id", question.Id));
                xQuestionRoot.Add(new XElement("question", question.Question));
                var xAnswerList = new XElement("answerList");
                foreach (var answer in question.AnswerList)
                {
                    var xAnswer = new XElement("answer");
                    xAnswer.Add(new XAttribute("id", answer.Id));
                    xAnswer.Add(new XElement("answer", answer.Answer));
                    xAnswer.Add(new XElement("isCorrectly", answer.IsCorrectly));
                    xAnswerList.Add(xAnswer);
                }
                xQuestionRoot.Add(xAnswerList);
                xQuestionList.Add(xQuestionRoot);
            }
            xTest.Add(xQuestionList);
            root.Add(xTest);
            File.Delete(Configurations.TestXML);
            xdoc.Save(Configurations.TestXML);
        }
        public static void SetTest(TestModel test)
        {
            var testList = GetTestList();
            var foundTest = testList.FirstOrDefault(p => p.Id == test.Id);

            foundTest.Name = test.Name;
            foundTest.Category = test.Category;
            foundTest.Description = test.Description;
            foundTest.QuestionList = test.QuestionList;

            File.Delete(Configurations.TestXML);
            if (!File.Exists(Configurations.TestXML))
                new XDocument(new XElement("tests")).Save(Configurations.TestXML);
            foreach (var item in testList)
            {
                AddTest(item);
            }
        }
        public static void DeleteTest(TestModel test)
        {
            XDocument xdoc = XDocument.Load(Configurations.TestXML);
            XElement root = xdoc.Element("tests");

            var foundTest = root.Elements("test").ToList().FirstOrDefault(p => p.Attribute("id").Value == test.Id.ToString());
            foundTest.Remove();

            File.Delete(Configurations.TestXML);
            xdoc.Save(Configurations.TestXML);
        }

        private static List<QuestionModel> parseQuestions(XmlNodeList childNodes)
        {
            List<QuestionModel> questions = new List<QuestionModel>();

            foreach(XmlNode root in childNodes)
            {
                QuestionModel questionModel = new QuestionModel();
                if (root.Attributes.Count > 0)
                {
                    XmlNode attr = root.Attributes.GetNamedItem("id");
                    if (attr != null)
                        questionModel.Id = long.Parse(attr.Value);
                }
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "question")
                        questionModel.Question = node.InnerText;
                    else if (node.Name == "answerList")
                        questionModel.AnswerList = parseAnswers(node.ChildNodes);
                }
                questions.Add(questionModel);
            }

            return questions;
        }
        private static List<AnswerModel> parseAnswers(XmlNodeList childNodes)
        {
            List<AnswerModel> answers = new List<AnswerModel>();
            foreach (XmlNode root in childNodes)
            {
                AnswerModel answerModel = new AnswerModel();
                if (root.Attributes.Count > 0)
                {
                    XmlNode attr = root.Attributes.GetNamedItem("id");
                    if (attr != null)
                        answerModel.Id = long.Parse(attr.Value);
                }
                foreach (XmlNode node in root.ChildNodes)
                {
                    if (node.Name == "answer")
                        answerModel.Answer = node.InnerText;
                    else if (node.Name == "isCorrectly")
                        answerModel.IsCorrectly = bool.Parse(node.InnerText);
                }
                answers.Add(answerModel);
            }
            return answers;
        }
    }
}
