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
    class GoEtran1
    {
        public string Str(string sborn)
        {
            var _url = "http://10.248.35.14:8092/AppServer/IEtranSysservice";
            var _action = "http://10.248.35.14:8092/AppServer/IEtranSysservice?op=HelloWorld";
            XmlDocument CreateSoap()
            {

                XmlDocument soapEnvelopeDocument = new XmlDocument();
                soapEnvelopeDocument.LoadXml(
                 "<soapenv:Envelope xmlns:soapenv='http://schemas.xmlsoap.org/soap/envelope/' xmlns:sys='SysEtranInt'>" +
                  "<soapenv:Body>" +
                 "<sys:GetBlock>" +
                 "<Login>ДУРАКОВ_ВА</Login>" +
                 "<Password>RO238533ye</Password>" +
                 "<Text>" +
                  sborn +
                "</Text>" +
                 "</sys:GetBlock>" +
                 "</soapenv:Body>" +
                 "</soapenv:Envelope> ");

                return soapEnvelopeDocument;
            }


            XmlDocument soapEnvelopeXml = CreateSoap();
            HttpWebRequest webRequest = CreateWebRequest(_url, _action);
            InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
            asyncResult.AsyncWaitHandle.WaitOne();
            string soapResult;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }

                return soapResult;               
            }
        }
            internal string Parsing(string sborn)
            {

            string otvet = "";
                GoEtran etr = new GoEtran();
                string perem = Str(sborn);
                Console.WriteLine("Размер файла " + perem.Length);
                string text = perem;
                var xml = XElement.Parse(text);
           //     var elem = XElement.Parse(xml.Value.Trim());
                string code3 = Regex.Replace(xml.ToString(), @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]", "");
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
        private static HttpWebRequest CreateWebRequest(string url, string action)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Timeout = 300000;
            //   webRequest.MaximumResponseHeadersLength = 2000000;
            webRequest.Headers.Add("SOAPAction", action);
            webRequest.ContentType = "text/xml;charset=\"utf-8\"";
            webRequest.Accept = "text/xml";
            webRequest.Method = "POST";
            //    webRequest.KeepAlive = false;
            return webRequest;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }



    }


}
