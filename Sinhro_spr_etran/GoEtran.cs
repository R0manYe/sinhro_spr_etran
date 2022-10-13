using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Sinhro_spr_etran
{
    class GoEtran
    {
        public string soapResult { get; set; }
        public int result { get; set; }
        public string otvet { get; set; }
        public string Str(string sborn)
        {
            WebRequest request = WebRequest.Create("http://192.168.1.125/Asu_proxy/Proxy.asmx/Zapros");
            request.Method = "POST";
            request.Timeout = 600000;
          //  sborn = "";
           
           
            string perem = "perem=" + sborn + "";
            File.WriteAllText("perem.xml", perem);

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(perem);

           request.ContentType = "application/x-www-form-urlencoded";

            request.ContentLength = byteArray.Length;


            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
           
                WebResponse response = request.GetResponse();
                string soapResult;
                using (Stream stream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        soapResult = reader.ReadToEnd();
                    }
                    File.WriteAllText("vag_har.xml", soapResult);
                    return soapResult;
                }
        /*    }
            catch (WebException ex)
            {
                string address = "roman@abakan.vspt.ru";
                string TextPisma = ex.ToString();
                EmOpov pi = new EmOpov();
                string Zagolovok = "Исключение при выполнениии запроса в GoEtran";
                pi.Opov_err(address, TextPisma, Zagolovok);
                return soapResult;
            }*/
        }
        internal string Parsing(string sborn)
        {
            GoEtran etr = new GoEtran();
            string perem = Str(sborn);
            Console.WriteLine("Размер файла " + perem.Length);
            string text = perem;
            var xml = XElement.Parse(text);
            var elem = XElement.Parse(xml.Value.Trim());
            string code3 = Regex.Replace(elem.ToString(), @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]", "");
            // Console.WriteLine(code3);
            File.WriteAllText("otvet0.xml", code3);
            XDocument xdoc = XDocument.Parse(code3);
            XNamespace a = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace b = "SysEtranInt";
            XNamespace c = "http://service.siw.pktbcki.rzd/";

            foreach (XElement VElement in xdoc.Element(a + "Envelope").Element(a + "Body").Elements(b + "GetBlockResponse"))
            {
                XAttribute nameAttribute = VElement.Attribute("value");
                XElement priceElement = VElement.Element("Text");
                string text1 = priceElement.ToString();
                var xml1 = XElement.Parse(text1);
                var elem1 = XElement.Parse(xml1.Value.Trim());
                otvet = Regex.Replace(elem1.ToString(), @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]", "");
                File.WriteAllText("otvet2.xml", otvet.ToString());
            }
            return otvet;
        }
        public int Proverka()
        {
            var _action = "http://192.168.1.125/Asu_proxy/Proxy.asmx/Zapros?Perem=<test></test>";

            WebRequest request = WebRequest.Create(_action);
            WebResponse response = request.GetResponse();

            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    soapResult = reader.ReadToEnd();
                }
                //    File.WriteAllText("vag_har.xml", soapResult);

            }
            var xml = XElement.Parse(soapResult);
            var elem = XElement.Parse(xml.Value.Trim());
            string code3 = Regex.Replace(elem.ToString(), @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]", "");
            // Console.WriteLine(code3);
            File.WriteAllText("otvet0.xml", code3);
            XDocument xdoc = XDocument.Parse(code3);
            XNamespace a = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace b = "SysEtranInt";
            XNamespace c = "http://service.siw.pktbcki.rzd/";

            foreach (XElement VElement in xdoc.Element(a + "Envelope").Element(a + "Body").Elements(b + "GetBlockResponse"))
            {
                XAttribute nameAttribute = VElement.Attribute("value");
                XElement priceElement = VElement.Element("Text");
                string text1 = priceElement.ToString();
                var xml1 = XElement.Parse(text1);
                var elem1 = XElement.Parse(xml1.Value.Trim());
                otvet = Regex.Replace(elem1.ToString(), @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]", "");
                File.WriteAllText("otvet2.xml", otvet.ToString());

            }
            string s2 = "<testReply>";
            result = string.Compare(s2, otvet.Substring(0, 11));


            return result;
        }


    }



}

