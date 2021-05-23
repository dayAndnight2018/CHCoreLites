using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MailLite
{
    public interface IMailSender
    {
        Task<bool> SendMail(MailAuth mailAuth, MailContent mailContent);
    }
}
