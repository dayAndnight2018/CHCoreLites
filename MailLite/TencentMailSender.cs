using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace MailLite
{
    public class TencentMailSender : IMailSender
    {
        private static readonly string SMTPSERVER = "smtp.qq.com";

        public async Task<bool> SendMail(MailAuth mailAuth, MailContent mailContent)
        {
            SmtpClient client = new SmtpClient(SMTPSERVER);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(mailAuth.Username, mailAuth.Password);
            
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(mailContent.Sender, mailContent.SenderDisplayName, Encoding.UTF8);
            mail.Subject = mailContent.Subject;
            mail.Body = mailContent.Content;
            foreach (var mailTo in mailContent.Receivers)
            {
                mail.To.Add(mailTo);
            }
            
            await client.SendMailAsync(mail);
            return true;
        }
    }
}
