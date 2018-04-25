namespace VessiFlowers.Business.Contracts
{
    using System.Threading.Tasks;
    using VessiFlowers.Business.Models.DomainModels;

    public interface IPageService
    {
        Task<PageViewModel> GetPage(string name);

        Task UpdatePage(PageViewModel pvm);

        Task<PageViewModel> GetBulletinPage(string months, string baseUrl, string blogImageUrl);

        Task<int> SendBulletinAsync(PageViewModel pvm);

    }
}
