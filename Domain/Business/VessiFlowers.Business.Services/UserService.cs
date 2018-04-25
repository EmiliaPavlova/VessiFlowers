namespace VessiFlowers.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using VessiFlowers.Business.Contracts;
    using VessiFlowers.Business.Models.UserModels;
    using VessiFlowers.Common.Utilities;
    using VessiFlowers.Data.Repositories;

    public class UserService : IUserService
    {
        private readonly InstanceFactory<VessiFlowersContext> contextFactory;

        public UserService(InstanceFactory<VessiFlowersContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<UserViewModel> GetUser(string id)
        {
            using (var context = this.contextFactory.Create())
            {
                return await context.Users.Where(u => u.Id == id).ProjectTo<UserViewModel>().FirstOrDefaultAsync();
            }
        }

        public async Task<IEnumerable<UserViewModel>> Read()
        {
            using (var context = this.contextFactory.Create())
            {
                return await context.Users.ProjectTo<UserViewModel>().ToListAsync();
            }
        }

        public async Task<bool> Update(UserViewModel uvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == uvm.Id);
                user.IsSubscribed = uvm.IsSubscribed;
                await context.SaveAsync();
                return true;
            }
        }
    }
}
