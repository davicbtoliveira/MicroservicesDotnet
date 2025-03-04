using Newtonsoft.Json;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace Common.Extensions.Logic
{
    public static class Extensao
    {
        public static XmlDocument ToXml(this Object objeto, XmlRootAttribute xRoot, Type tipo)
        {
            XmlDocument retorno = new XmlDocument();
            XmlWriterSettings sets = new XmlWriterSettings();
            sets.NamespaceHandling = NamespaceHandling.OmitDuplicates;

            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", xRoot.Namespace);

            var xmlSerializer = new XmlSerializer(tipo, xRoot);
            using (var memStm = new MemoryStream())
            using (var xw = XmlWriter.Create(memStm))
            {
                xmlSerializer.Serialize(xw, objeto, ns);
                memStm.Position = 0;
                retorno.Load(memStm);
            }
            return retorno;
        }

        public static string ToXML<TClass>(this TClass classe)
        {
            var stringwriter = new System.IO.StringWriter();
            var serializer = new XmlSerializer(classe.GetType());
            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            serializer.Serialize(stringwriter, classe, ns);

            var xml = stringwriter.ToString();
            xml = xml.Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n", "");

            return xml;
        }


        public static string soNumeros(this string toNormalize)
        {
            List<char> numbers = new List<char>("0123456789");
            StringBuilder toReturn = new StringBuilder(toNormalize.Length);
            CharEnumerator enumerator = toNormalize.GetEnumerator();

            while (enumerator.MoveNext())
            {
                if (numbers.Contains(enumerator.Current))
                    toReturn.Append(enumerator.Current);
            }

            return toReturn.ToString();
        }

        public static string GetNumeros(string texto)
        {
            return string.IsNullOrEmpty(texto) ? "" : new String(texto.Where(Char.IsDigit).ToArray());
        }

        public static string GetQueryString(this object obj)
        {
            var step1 = JsonConvert.SerializeObject(obj);

            var step2 = JsonConvert.DeserializeObject<IDictionary<string, string>>(step1);

            var step3 = step2.Select(x => HttpUtility.UrlEncode(x.Key) + "=" + HttpUtility.UrlEncode(x.Value));

            return string.Join("&", step3);
        }
    }
}
