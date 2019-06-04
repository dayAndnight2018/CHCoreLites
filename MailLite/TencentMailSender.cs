using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailLite
{
    public class TencentMailSender : IMailSender
    {
        private static readonly string SMTPSERVER = "smtp.qq.com";

        public async Task<bool> SendMail(string userName, string password, string senderAddress, string senderDisplayName, string toMailAddress, string subject, string content)
        {
            SmtpClient client = new SmtpClient(SMTPSERVER);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(userName, password);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(senderAddress, senderDisplayName, Encoding.UTF8);

            mail.Subject = $"【{subject}】";
            mail.Body = content;
            mail.To.Add(toMailAddress);
            await client.SendMailAsync(mail);
            return true;
        }
    }
}
