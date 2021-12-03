using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SystemControl.Services
{
    class FileService
    {
        public static string AppStartPath { get
            {
                string path = Assembly.GetExecutingAssembly().Location;
                string[] tmp = path.Split('\\');
                string res = path.Replace("\\"+tmp.Last(), "");
                return res;
            }
        }
        public static void CreatingFileStructure()
        {
            Directory.CreateDirectory(Configurations.AppRootDir);
            if(!File.Exists(Configurations.UserXML))
                new XDocument(new XElement("users")).Save(Configurations.UserXML);
            if (!File.Exists(Configurations.TestXML))
                new XDocument(new XElement("tests")).Save(Configurations.TestXML);
            if (!File.Exists(Configurations.StatisticsXML))
                new XDocument(new XElement("statistics")).Save(Configurations.StatisticsXML);
        }
    }
}
