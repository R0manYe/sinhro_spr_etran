using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Sinhro_spr_etran
{
    class EmOpov
    {
        public void Opov_err(string address, string TextPisma, string Zagolovok)
        {

            DateTime parsedDate = DateTime.Now;
            SmtpClient Smtp = new SmtpClient("mail.vspt.org", 25);
            Smtp.EnableSsl = false;
            Smtp.Credentials = new System.Net.NetworkCredential("robot", "Abakan_mail18");
            MailMessage MyMessage = new MailMessage();
            MyMessage.From = new MailAddress("robot@abakan.vspt.ru");
            MyMessage.To.Add(address);
            MyMessage.Subject = Zagolovok;
            MyMessage.Body = " Здравтсвуйте! \n " +
            TextPisma;
            Smtp.Send(MyMessage);

        }
    }
}
