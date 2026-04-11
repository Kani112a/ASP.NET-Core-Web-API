namespace Cityinfo.API.Service
{
    public class LocalMailService : IMailService
    {
        private string _frommail = string.Empty;
        private string _tomail = string.Empty;
        public LocalMailService(IConfiguration configuration)
        {
            _frommail = configuration["mailSettings:mailFromAddress"];
            _tomail = configuration["mailSettings:mailToAddress"];
        }
        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_frommail} to {_tomail}, with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject :{subject}");
            Console.WriteLine($"Message :{message}");
        }
    }
}
