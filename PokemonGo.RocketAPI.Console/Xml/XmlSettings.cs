using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PokemonGo.RocketAPI.Console.Xml
{
    public static class XmlSettings
    {
        public static readonly string _configsPath = Path.Combine(Directory.GetCurrentDirectory(), "Settings");
        public static readonly string _configsFile = Path.Combine(_configsPath, "Settings.xml");
        public static void CreateSettings(ISettings settings)
        {
            if (!Directory.Exists(_configsPath)) Directory.CreateDirectory(_configsPath);
            if (File.Exists(_configsFile)) return;

            File.Create(_configsFile).Close();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(XmlSerialize.ClassToXml(settings, "Settings"));
            doc.Save(_configsFile);
        }

        public static Settings LoadSettings()
        {
            var profileStream = File.Open(_configsFile, FileMode.Open);
            return XmlSerialize.XmlToClass<Settings>(new StreamReader(profileStream).ReadToEnd());
        }
    }
}
