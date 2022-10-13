using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sinhro_spr_etran
{
    class Spr_etran_station
    {
        private int i { get; set; }
        public void Ses()
        {
            string del_freight = "delete from SPR_ETRAN_station_temp";
            string stroka = del_freight;
            Sql_z sql1 = new Sql_z();
            sql1.Oracle_v(stroka);
            bool cicl = true;

            for (i = 12010; (cicl == true); i += 200)
            {
                //  Console.WriteLine(cicl);
                ///  Console.WriteLine(i);
                Create_zapros Zapros = new Create_zapros();
                string rez_z = Zapros.Ses(i, out string pr);
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
                    Console.WriteLine("Spr_etran_station");
                    string stroka1 = "DECLARE @x xml SET @x ='" + code1.ToString() + "' INSERT INTO [FLAGMAN]..[VSPTSVOD].[SPR_ETRAN_STATION_TEMP] ([ST_ID],[RECDATEBEGIN],[RECDATEEND],[ST_CODE],[ST_CODE_OSJD],[ST_DP_ID],[ST_STT_ID],[ST_RG_ID],[ST_DP_RW_ID],[ST_CN_ID],[ST_NAME_12CHAR],[ST_NAME])" +
                                " (SELECT " +
                            "T.c.value('(ST_ID/@value)[1]', 'int') AS ST_ID, " +
                            "T.c.value('(RECDATEBEGIN/@value)[1]', 'date') AS RECDATEBEGIN, " +
                            "T.c.value('(RECDATEEND/@value)[1]', 'date') AS RECDATEEND, " +
                            "T.c.value('(ST_CODE/@value)[1]', 'int') AS ST_CODE, " +
                             "T.c.value('(ST_CODE_OSJD/@value)[1]', 'int') AS ST_CODE_OSJD, " +
                            "T.c.value('(ST_DP_ID/@value)[1]', 'int') AS ST_DP_ID, " +
                            "T.c.value('(ST_STT_ID/@value)[1]', 'int') AS ST_STT_ID, " +
                            "T.c.value('(ST_RG_ID/@value)[1]', 'int') AS ST_RG_ID, " +
                            "T.c.value('(ST_DP_RW_ID/@value)[1]', 'int') ST_DP_RW_ID, " +
                            "T.c.value('(ST_CN_ID/@value)[1]', 'int') AS ST_CN_ID, " +
                            "T.c.value('(ST_NAME_12CHAR/@value)[1]', 'varchar(50)') AS ST_NAME_12CHAR, " +
                            "T.c.value('(ST_NAME/@value)[1]', 'varchar(250)') AS ST_NAME " +
                            "FROM @x.nodes('/getNSIReply/row') T(c))";
                    sql1.Mssql_v(stroka1);
                }
                else
                {
                    cicl = false;
                }
            }
            if (i > 20000)
            {
                string del = "delete from SPR_ETRAN_STATION";
                string ins_temp = "insert into spr_etran_station(ST_ID,RECDATEBEGIN,RECDATEEND,ST_CODE,ST_DP_ID,ST_STT_ID,ST_RG_ID,ST_DP_RW_ID,ST_CN_ID,ST_NAME_12CHAR,ST_NAME,ST_CODE_OSJD,data_ins)" +
                              " SELECT ST_ID, RECDATEBEGIN, RECDATEEND, ST_CODE, ST_DP_ID, ST_STT_ID, ST_RG_ID, ST_DP_RW_ID, ST_CN_ID, ST_NAME_12CHAR, ST_NAME, ST_CODE_OSJD, sysdate FROM spr_etran_station_temp where ST_CODE is not null";
                string ins_log = "insert into spr_etran_log (data,col_z,cikl,spr) values (sysdate,(select count(*) from spr_etran_station),'" + i + "','SPR_ETRAN_STATION')";
                sql1.Oracle_v(del);
                sql1.Oracle_v(ins_temp);
                sql1.Oracle_v(ins_log);
                //{
                //    string address = "roman@abakan.vspt.ru";
                //    string TextPisma = "Таблица SPR_ETRAN_STATION  синхронизирована.Ошибок нет";
                //    EmOpov pi = new EmOpov();
                //    string Zagolovok = "Таблица SPR_ETRAN_STATION  синхронизирована.";
                //    pi.Opov_err(address, TextPisma, Zagolovok);
                //}


            }
            else
            {
                string address = "roman@abakan.vspt.ru";
                string TextPisma = "Данных менее 20000 записей что подозрительно мало.Таблица SPR_ETRAN_STATION не синхронизирована.";
                EmOpov pi = new EmOpov();
                string Zagolovok = "Ошибка при импорте справочника SPR_ETRAN_STATION";
                pi.Opov_err(address, TextPisma, Zagolovok);
            }

        }

    }
}
