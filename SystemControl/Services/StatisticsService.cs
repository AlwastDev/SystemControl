using SystemControl.Models;
using System.Collections.Generic;
using System.Linq;

namespace SystemControl.Services
{
    static class StatisticsService
    {
        public static void Add(StatisticsModel statistics)
        {
            XmlService.AddStatistics(statistics);
        }

        public static List<StatisticsModel> GetList()
        {
            return XmlService.GetStatistics().Where(p => p.UserId == UserService.AuthUser.Id).ToList();
        }

    }
}
