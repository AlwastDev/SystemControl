using SystemControl.Models.TestModels;
using System.Collections.Generic;
using System.Linq;

namespace SystemControl.Services
{
    static class TestService
    {
        public static List<TestModel> GetAllTests()
        {
            return XmlService.GetTestList();
        }

        public static List<TestModel> GetMyTests()
        {
            return XmlService.GetTestList().Where(p => p.AuthorId == UserService.AuthUser.Id).ToList();
        }

        public static void Create(TestModel testModel)
        {
            XmlService.AddTest(testModel);
        }

        public static void Edit(TestModel testModel)
        {
            XmlService.SetTest(testModel);
        }

        public static void Delete(TestModel testModel)
        {
            var statistics = XmlService.GetStatistics().Where(p => p.TestId == testModel.Id);
            foreach(var item in statistics)
                XmlService.DeleteStatistics(item);
            XmlService.DeleteTest(testModel);
        }
    }
}
