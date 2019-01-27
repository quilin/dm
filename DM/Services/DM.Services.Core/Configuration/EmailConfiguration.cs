namespace DM.Services.Core.Configuration
{
    public class EmailConfiguration
    {
        public string ServerHost { get; set; }
        public int ServerPort { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public string ReplyToAddress { get; set; }
    }
}