namespace MailLite
{
    public class MailAuth
    {
        private string username;
        private string password;

        public string Username => username;

        public string Password => password;

        public override string ToString()
        {
            return $"{nameof(Username)}: {Username}, {nameof(Password)}: {Password}";
        }
    }
}