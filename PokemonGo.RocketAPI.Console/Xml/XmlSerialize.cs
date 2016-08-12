using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PokemonGo.RocketAPI.Console.Xml
{
    public static class XmlSerialize
    {

        public static T XmlToClass<T>(string xml) where T : new()
        {
            try
            {
                var xmlSerz = new XmlSerializer(typeof(T));
                using (var strReader = new StringReader(xml))
                {
                    var obj = xmlSerz.Deserialize(strReader);
                    return (T)obj;
                }
            }
            catch
            {
                return default(T);
            }
        }

        public static string ClassToXml(object obj, string root)
        {
            var xns = new XmlSerializerNamespaces();
            xns.Add(string.Empty, string.Empty);

            if (null == obj) return string.Empty;
            var ser = new XmlSerializer(obj.GetType(), new XmlRootAttribute(root));

            var writer = new StringWriter();
            var xmlWriter = XmlWriter.Create(writer, new XmlWriterSettings { OmitXmlDeclaration = true });
            ser.Serialize(xmlWriter, obj, xns);
            return writer.ToString();
        }

    }
}
