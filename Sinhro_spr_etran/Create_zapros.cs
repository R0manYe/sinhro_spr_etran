using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Sinhro_spr_etran
{
    public class Create_zapros
    {

        public string Seft(in int i, out string pr)
        {
                string sborn = "<getNSI>" +
                     "<tableName value=\"FREIGHT\"/>" +
                     "<filterField value=\"FR_id,fr_id\"/>" +
                     "<filterCond value='&gt;=,&lt;='/>" +
                     "<filterValue value='" + (i + 1 - 200) + "," + i + "'/>" +
                "</getNSI>";
                Console.WriteLine(i);
              //  File.WriteAllText("ott88.xml", sborn);
                GoEtran1 otv = new GoEtran1();
                pr = otv.Parsing(sborn);
                File.WriteAllText("ott99.xml", pr);
                return pr;
            
        }
        public string Segf_2001(in int i, out string pr)
        {
            string sborn = "<getNSI>" +
                "<tableName value = 'GNG_FREIGHT_2001' /> " +
                "<filterField value = 'gf_id,gf_id'/>" +
                "<filterCond value = '&gt;=,&lt;='/>" +
                "<filterValue value = '" + (i + 1 - 200) + "," + i + "'/>"+
            "</getNSI>";
            Console.WriteLine(i);
            //  File.WriteAllText("ott88.xml", sborn);
            GoEtran1 otv = new GoEtran1();
            pr = otv.Parsing(sborn);
            File.WriteAllText("ott99.xml", pr);
            return pr;

        }
        public string Ses(in int i, out string pr)
        {
            string sborn ="<getNSI version='1.0'>" +
                    "<tableName value = 'STATION'/>" +
                    "<filterField value = 'st_id,st_id'/>" +
                    "<filterCond value = '&gt;=,&lt;='/>" +
                    "<filterValue value = '" + (i + 1 - 200) + "," + i + "'/>" +
                    "</getNSI>";
            Console.WriteLine(i);
            //  File.WriteAllText("ott88.xml", sborn);
            GoEtran1 otv = new GoEtran1();
            pr = otv.Parsing(sborn);
            File.WriteAllText("ott99.xml", pr);
            return pr;

        }
        public string Sesf(string pr)
        {
            string sborn="<getNSI version = '1.0' > " +
                "<tableName value = 'SUM_FREIGHT'/>" +
                "</getNSI>";
           // Console.WriteLine(i);
            //  File.WriteAllText("ott88.xml", sborn);
            GoEtran1 otv = new GoEtran1();
            pr = otv.Parsing(sborn);
            File.WriteAllText("ott99.xml", pr);
            return pr;

        }




    }
}
