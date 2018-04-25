namespace VessiFlowers.Business.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VessiFlowers.Business.Models.UserModels;

    public interface IUserService
    {
        Task<UserViewModel> GetUser(string id);

        Task<IEnumerable<UserViewModel>> Read();

        Task<bool> Update(UserViewModel uvm);
    }
}
