namespace Cityinfo.API.Service
{
    public class LocalMailService
    {
        private string _frommail = "kani@gmail.com";
        private string _tomail = "pavi@gmail.com";
        public void Send(string subject, string message)
        {
            Console.WriteLine($"Mail from {_frommail} to {_tomail}, with {nameof(LocalMailService)}");
            Console.WriteLine($"Subject :{subject}");
            Console.WriteLine($"Message :{message}");
        }
    }
}
