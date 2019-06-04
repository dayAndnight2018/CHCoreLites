using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailLite
{
    public interface IMailSender
    {
        Task<bool> SendMail(string userName, string password, string senderAddress, string senderDisplayName, string toMailAddress, string subject, string content);
    }
}
