namespace Cityinfo.API.Service
{
    public class CloudMailService : IMailService
    {
        private string _frommail = "pavithrakanishka2002@gmail.com";
        private string _tomail = "kanishkadurai03@gmail.com";
        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_frommail} to {_tomail}, with {nameof(CloudMailService)}");
            Console.WriteLine($"Subject :{subject}");
            Console.WriteLine($"Message :{message}");
        }
    }
}
