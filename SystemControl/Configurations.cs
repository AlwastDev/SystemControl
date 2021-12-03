using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemControl.Services;

namespace SystemControl
{
    static class Configurations
    {
        public static string AppRootDir { get => $@"{FileService.AppStartPath}\resources"; }
        public static string UserXML { get => $@"{AppRootDir}\users.xml"; }
        public static string TestXML { get => $@"{AppRootDir}\tests.xml"; }
        public static string StatisticsXML { get => $@"{AppRootDir}\statistics.xml"; }
    }
}
