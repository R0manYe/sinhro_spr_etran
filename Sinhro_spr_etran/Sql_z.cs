using Oracle.DataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinhro_spr_etran
{
    class Sql_z
    {
        public void Oracle_v(string stroka)
        {
            using (OracleConnection conn = new OracleConnection("Data Source = flagman; Persist Security Info=True;User ID = vsptsvod; Password=sibpromtrans"))
            {
                OracleCommand command = new OracleCommand(stroka, conn);
                conn.Open();
                OracleDataReader vivod = command.ExecuteReader();
                conn.Close();
            }

        }
        public void Mssql_v(string stroka)
        {
            using (SqlConnection connection1 = new SqlConnection("Data Source=192.168.1.13;Initial Catalog=dislokacia;User ID=Roman;Password=238533"))
            {
                SqlCommand command3 = new SqlCommand(stroka, connection1);
                connection1.Open();
                SqlDataReader thisStroka2 = command3.ExecuteReader();
                connection1.Close();
            }


        }
    }
}
