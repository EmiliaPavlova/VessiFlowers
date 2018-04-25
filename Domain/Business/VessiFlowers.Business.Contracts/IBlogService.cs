namespace VessiFlowers.Business.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VessiFlowers.Business.Models.DomainModels;

    public interface IBlogService
    {
        Task<BlogViewModel> GetBlog(int page);

        IEnumerable<PostViewModel> ReadPosts();

        Task<PostViewModel> GetPost(int id);

        Task<int> CreatePost(PostViewModel pvm);

        Task<int> UpdatePost(PostViewModel pvm);

        Task DeletePost(int id);

        Task<MediaViewModel> GetMedia(int id, int pid = 0);

        Task CreateMedia(MediaViewModel mvm);

        Task UpdateMedia(MediaViewModel mvm);

        Task DeleteMeldia(int id);
    }
}
