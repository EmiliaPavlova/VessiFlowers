namespace VessiFlowers.Business.Contracts
{
    public interface IEmailService
    {
        void Send(string from, string to, string subject, string body, bool isHtml = false);
    }
}
