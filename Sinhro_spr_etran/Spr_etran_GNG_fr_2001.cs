using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sinhro_spr_etran
{
   
    class Spr_etran_GNG_fr_2001
    {
        private int i { get; set; }
        public void Segf_2001()
        {
            string del_freight = "delete from SPR_ETRAN_GNG_FR_2001_TEMP";
            string stroka = del_freight;
            Sql_z sql1 = new Sql_z();
            sql1.Oracle_v(stroka);
            bool cicl = true;

            for (i = 12010; (cicl == true); i += 200)
            {
                //  Console.WriteLine(cicl);
                ///  Console.WriteLine(i);
                Create_zapros Zapros = new Create_zapros();
                string rez_z = Zapros.Segf_2001( i, out string pr);
             //   Console.WriteLine(rez_z);
                XDocument xdoc = XDocument.Parse(rez_z);
                XElement getNSIReply = xdoc.Element("getNSIReply").Element("row");
                if (getNSIReply != null)
                {
                    cicl = true;
                    var xml = XElement.Parse(rez_z);
                    // var elem = XElement.Parse(xml.Value.Trim());
                    string code3 = Regex.Replace(xml.ToString(), @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]", "");
                    string code1 = code3.Replace("'", "");
                    Console.WriteLine("Spr_etran_GNG_fr_2001");
                    string stroka1 = "DECLARE @x xml SET @x ='" + code1.ToString() + "'INSERT INTO[FLAGMAN]..[VSPTSVOD].[SPR_ETRAN_GNG_FR_2001_TEMP] " +
                           "([GF_ID],[RECDATEBEGIN],[RECDATEEND],[GF_CODE],[GF_NAME],[ETSNG],[GF_CODE_OLD],[FR_ID])" +
                           "(SELECT " +
                           "T.c.value('(GF_ID/@value)[1]', 'varchar(100)') AS GF_ID, " +
                           "T.c.value('(RECDATEBEGIN/@value)[1]', 'date') AS RECDATEBEGIN, " +
                           "T.c.value('(RECDATEEND/@value)[1]', 'date') AS RECDATEEND, " +
                          "T.c.value('(GF_CODE/@value)[1]', 'varchar(200)') AS GF_CODE, " +
                           "T.c.value('(GF_NAME/@value)[1]', 'varchar(1000)') AS GF_NAME, " +
                           "T.c.value('(ETSNG/@value)[1]', 'varchar(30)') AS ETSNG, " +
                           "T.c.value('(GF_OLD_CODE/@value)[1]', 'varchar(20)') GF_OLD_CODE, " +
                           "T.c.value('(FR_ID/@value)[1]', 'int') AS FR_ID " +
                           "FROM @x.nodes('/getNSIReply/row') T(c))";
                    sql1.Mssql_v(stroka1);
                }
                else
                {
                    cicl = false;
                }
            }
            if (i > 4000)
            {
                string del = "delete from SPR_ETRAN_GNG_FREIGHT_2001";
                string ins_temp = "insert into SPR_ETRAN_GNG_FREIGHT_2001(GF_ID,RECDATEBEGIN,RECDATEEND,GF_CODE,GF_NAME,ETSNG,GF_CODE_OLD,FR_ID) " +
                                   " SELECT GF_ID, RECDATEBEGIN, RECDATEEND, GF_CODE, GF_NAME, ETSNG, GF_CODE_OLD, FR_ID FROM SPR_ETRAN_GNG_FR_2001_TEMP";
                string ins_log = "insert into spr_etran_log (data,col_z,cikl,spr) values (sysdate,(select count(*) from SPR_ETRAN_GNG_FREIGHT_2001),'" + i + "','GNG_FREIGHT_2001')";
                sql1.Oracle_v(del);
                sql1.Oracle_v(ins_temp);
                sql1.Oracle_v(ins_log);
            }
            else
            {
                string address = "roman@abakan.vspt.ru";
                string TextPisma = "Данных менее 4000 записей что подозрительно мало.Таблица SPR_ETRAN_GNG_FREIGHT_2001 не синхронизирована.";
                EmOpov pi = new EmOpov();
                string Zagolovok = "Ошибка при импорте справочника SPR_ETRAN_GNG_FREIGHT_2001";
                pi.Opov_err(address, TextPisma, Zagolovok);
            }

        }

    }
}

