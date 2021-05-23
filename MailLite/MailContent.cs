using System.Collections.Generic;

namespace MailLite
{
    public class MailContent
    {
        private string sender;
        private string senderDisplayName;
        private List<string> receivers;

        private string subject;
        private string content;

        public string Sender => sender;
        public string SenderDisplayName => senderDisplayName;
        public List<string> Receivers => receivers;
        
        public string Subject => subject;
        public string Content => content;

        public MailContent(string sender, string senderDisplayName, List<string> receivers, string subject, string content)
        {
            this.sender = sender;
            this.senderDisplayName = senderDisplayName;
            this.receivers = receivers;
            this.subject = subject;
            this.content = content;
        }

        public MailContent()
        {
        }
    }
}