using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sinhro_spr_etran
{
    class Spr_etran_sum_freight
    {
        private int i { get; set; }
        private string pr { get; set; }
        public void Sesf()
        {
           
            Sql_z sql1 = new Sql_z();
           

            bool cicl = true;
                Create_zapros Zapros = new Create_zapros();
                string rez_z = Zapros.Sesf(pr);
                //    Console.WriteLine(rez_z);
                XDocument xdoc = XDocument.Parse(rez_z);
                XElement getNSIReply = xdoc.Element("getNSIReply").Element("row");
            if (getNSIReply != null)
            {
                cicl = true;
                var xml = XElement.Parse(rez_z);
                // var elem = XElement.Parse(xml.Value.Trim());
                string code3 = Regex.Replace(xml.ToString(), @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]", "");
                string code1 = code3.Replace("'", "");
                if (code1.Length > 18000)
                {

                    Console.WriteLine("Spr_etran_sum_freight");
                    string delete = "delete from SPR_ETRAN_SUM_FREIGHT";
                    string stroka1 = "DECLARE @x xml SET @x ='" + code1.ToString() + "' INSERT INTO [FLAGMAN]..[VSPTSVOD].[SPR_ETRAN_SUM_FREIGHT]" +
                               "([GG_NUMBER],[RECDATEBEGIN],[RECDATEEND],[GG_NAME],[GG_CHAR_NAME],[STAT_MIN],[STAT_MAX],[GG_TYPE],[GG_NEED_WEIGHT])" +
                               "(SELECT " +
                               "T.c.value('(GG_NUMBER/@value)[1]', 'int') AS GG_NUMBER, " +
                               "T.c.value('(RECDATEBEGIN/@value)[1]', 'date') AS RECDATEBEGIN," +
                               "T.c.value('(RECDATEEND/@value)[1]', 'date') AS RECDATEEND," +
                               "T.c.value('(GG_NAME/@value)[1]', 'varchar(300)') AS GG_NAME," +
                               "T.c.value('(GG_CHAR_NAME/@value)[1]', 'varchar(20)') AS GG_CHAR_NAME," +
                               "T.c.value('(STAT_MIN/@value)[1]', 'int') AS STAT_MIN," +
                               "T.c.value('(STAT_MAX/@value)[1]', 'int') ASSTAT_MAX," +
                               "T.c.value('(GG_TYPE/@value)[1]', 'int') AS GG_TYPE," +
                               "T.c.value('(GG_NEED_WEIGHT/@value)[1]', 'int') AS GG_NEED_WEIGHT " +
                               "FROM @x.nodes('/getNSIReply/row') T(c))";
                    string ins_log = "insert into spr_etran_log (data,col_z,cikl,spr) values (sysdate,(select count(*) from SPR_ETRAN_SUM_FREIGHT),'1','SUM_FREIGHT')";

                    sql1.Oracle_v(delete);
                    sql1.Mssql_v(stroka1);
                    sql1.Oracle_v(ins_log);
                }
                else
                {
                    string address = "roman@abakan.vspt.ru";
                    string TextPisma = "Данных менее 20000 записей что подозрительно мало.Таблица SPR_ETRAN_SUM_FREIGH не синхронизирована.";
                    EmOpov pi = new EmOpov();
                    string Zagolovok = "Ошибка при импорте справочника SPR_ETRAN_SUM_FREIGH";
                    pi.Opov_err(address, TextPisma, Zagolovok);
                }
            }

        }
    }
}
