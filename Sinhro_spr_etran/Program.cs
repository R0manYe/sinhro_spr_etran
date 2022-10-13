using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinhro_spr_etran
{
    class Program
    {
        static void Main(string[] args)
        {
            GoEtran ch = new GoEtran();
            var otv = ch.Proverka();
            Console.WriteLine(otv);
            if (otv == 0)
            {

                  Spr_etran_freight zap = new Spr_etran_freight();
                  zap.SERT();
                  Spr_etran_GNG_fr_2001 zap1 = new Spr_etran_GNG_fr_2001();
                  zap1.Segf_2001();
                  Spr_etran_station zap2 = new Spr_etran_station();
                  zap2.Ses();
                Spr_etran_sum_freight zap3 = new Spr_etran_sum_freight();
                zap3.Sesf();

            }
        }
    }
}
