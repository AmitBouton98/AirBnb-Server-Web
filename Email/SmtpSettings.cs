namespace Server.Email
{
    public class SmtpSettings
    {
        public SmtpSettings(string host, int port, bool enableSsl, string userName, string password)
        {
            Host = host;
            Port = port;
            EnableSsl = enableSsl;
            UserName = userName;
            Password = password;
        }

        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }

}
