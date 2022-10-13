using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sinhro_spr_etran
{
    class Spr_etran_freight
    {
        private int i { get; set; }
        public void SERT()
        {
            string del_freight = "delete from SPR_ETRAN_FREIGHT_TEMP";
            string stroka = del_freight;
            Sql_z sql1 = new Sql_z();
            sql1.Oracle_v(stroka);
            bool cicl = true;

            for (i = 200; (cicl == true); i += 200)
            {
                //  Console.WriteLine(cicl);
                ///  Console.WriteLine(i);
                Create_zapros Zapros = new Create_zapros();
                string rez_z = Zapros.Seft(i, out string pr);
                // Console.WriteLine(rez_z);             
                XDocument xdoc = XDocument.Parse(rez_z);
                XElement getNSIReply = xdoc.Element("getNSIReply").Element("row");
                if (getNSIReply != null)
                {
                    cicl = true;
                    var xml = XElement.Parse(rez_z);
                    // var elem = XElement.Parse(xml.Value.Trim());
                    string code3 = Regex.Replace(xml.ToString(), @"[\u0000-\u0008,\u000B,\u000C,\u000E-\u001F]", "");
                    string code1 = code3.Replace("'", "");
                    Console.WriteLine("SPR_ETRAN_FREIGHT_TEMP");
                    string stroka1 = "DECLARE @x xml SET @x ='" + code1.ToString() + "' INSERT INTO[FLAGMAN]..[VSPTSVOD].[SPR_ETRAN_FREIGHT_TEMP] " +
                                      "([INS_DATE],[FR_ID],[RECDATEBEGIN],[RECDATEEND],[FR_CODE_ETSNG],[FR_GG_NUMBER],[FR_SHORT_NAME],[FR_NAME],[FR_CLASS],[FR_GO6_ID],[FR_GO10_ID],[FR_CO11_ID],[GUARD],[SKOROPORT],[NAVAL],[NASIP],[NALIV],[KARANTIN],[OPENVAGONS],[SOPROVOD],[SMERZ],[ALPH_EXIST],[EXACT_NAME],[FR_CO15_ID])" +
                                      "(SELECT GETDATE()," +
                                      "T.c.value('(FR_ID/@value)[1]', 'int') AS FR_ID," +
                                      "T.c.value('(RECDATEBEGIN/@value)[1]', 'date') AS RECDATEBEGIN," +
                                      "T.c.value('(RECDATEEND/@value)[1]', 'date') AS RECDATEEND," +
                                      "T.c.value('(FR_CODE_ETSNG/@value)[1]', 'varchar(6)') AS FR_CODE_ETSNG," +
                                      "T.c.value('(FR_GG_NUMBER/@value)[1]', 'int') AS FR_GG_NUMBER," +
                                      "T.c.value('(FR_SHORT_NAME/@value)[1]', 'varchar(20)') AS FR_SHORT_NAME," +
                                      "T.c.value('(FR_NAME/@value)[1]', 'varchar(260)') AS FR_NAME," +
                                      "T.c.value('(FR_CLASS/@value)[1]', 'varchar(20)') AS FR_CLASS," +
                                      "T.c.value('(FR_GO6_ID/@value)[1]', 'int') AS FR_GO6_ID," +
                                      "T.c.value('(FR_GO10_ID/@value)[1]', 'int') AS FR_GO10_ID," +
                                      "T.c.value('(FR_CO11_ID/@value)[1]', 'int') AS FR_CO11_ID," +
                                      "T.c.value('(GUARD/@value)[1]', 'varchar(20)') AS GUARD," +
                                      "T.c.value('(SKOROPORT/@value)[1]', 'varchar(20)') AS SKOROPORT," +
                                      "T.c.value('(NALIV/@value)[1]', 'varchar(4)') AS NALIV," +
                                      "T.c.value('(NAVAL/@value)[1]', 'varchar(4)') AS NAVAL," +
                                      "T.c.value('(NASIP/@value)[1]', 'varchar(4)') AS NASIP," +
                                      "T.c.value('(KARANTIN/@value)[1]', 'varchar(20)') AS KARANTIN," +
                                      "T.c.value('(OPENVAGONS/@value)[1]', 'varchar(20)') AS OPENVAGONS," +
                                      "T.c.value('(SOPROVOD/@value)[1]', 'varchar(20)') AS SOPROVOD," +
                                      "T.c.value('(SMERZ/@value)[1]', 'varchar(20)') AS SMERZ," +
                                      "T.c.value('(ALPH_EXIST/@value)[1]', 'varchar(20)') AS ALPH_EXIST," +
                                      "T.c.value('(EXACT_NAME/@value)[1]', 'varchar(20)') AS EXACT_NAME," +
                                      "T.c.value('(FR_CO15_ID/@value)[1]', 'int') AS carMODEL_CODE " +
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
                string del = "delete from SPR_ETRAN_FREIGHT";
                string ins_temp = "INSERT INTO SPR_ETRAN_FREIGHT (FR_ID,RECDATEBEGIN,RECDATEEND,FR_CODE_ETSNG,FR_GG_NUMBER,FR_SHORT_NAME,FR_NAME,FR_CLASS,FR_GO6_ID,FR_GO10_ID," +
                    " FR_CO11_ID,GUARD,SKOROPORT,NAVAL,NASIP,NALIV,KARANTIN,OPENVAGONS,SOPROVOD,SMERZ,ALPH_EXIST,EXACT_NAME,FR_CO15_ID,INS_DATE) select  FR_ID,RECDATEBEGIN," +
                    " RECDATEEND,FR_CODE_ETSNG,FR_GG_NUMBER,FR_SHORT_NAME,FR_NAME,FR_CLASS,FR_GO6_ID,FR_GO10_ID,FR_CO11_ID,GUARD,SKOROPORT,NAVAL,NASIP,NALIV,KARANTIN,OPENVAGONS," +
                    " SOPROVOD,SMERZ,ALPH_EXIST,EXACT_NAME,FR_CO15_ID,SYSDATE from  SPR_ETRAN_FREIGHT_TEMP";
                string ins_log = "insert into spr_etran_log (data,col_z,cikl,spr) values (sysdate,(select count(*) from SPR_ETRAN_FREIGHT),'" + i + "','SPR_ETRAN_FREIGHT')";
                sql1.Oracle_v(del);
                sql1.Oracle_v(ins_temp);
                sql1.Oracle_v(ins_log);

            }
            else
            {
                string address = "roman@abakan.vspt.ru";
                string TextPisma = "Данных менее 4000 записей что подозрительно мало.Таблица SPR_ETRAN_FREIGH не синхронизирована.";
                EmOpov pi = new EmOpov();
                string Zagolovok = "Ошибка при импорте справочника SPR_ETRAN_FREIGH";
                pi.Opov_err(address, TextPisma, Zagolovok);


            }
        }

    }
}

