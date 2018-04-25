namespace VessiFlowers.Business.Services
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper.QueryableExtensions;
    using VessiFlowers.Business.Contracts;
    using VessiFlowers.Business.Models.DomainModels;
    using VessiFlowers.Common.Utilities;
    using VessiFlowers.Data.Entities;
    using VessiFlowers.Data.Repositories;

    public class BlogService : IBlogService
    {
        private readonly InstanceFactory<IVessiFlowersContext> contextFactory;

        public BlogService(InstanceFactory<IVessiFlowersContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public IEnumerable<PostViewModel> ReadPosts()
        {
            using (var context = this.contextFactory.Create())
            {
                return context.Posts.OrderByDescending(p => p.Created)
                                          .ProjectTo<PostViewModel>()
                                          .ToList();
            }
        }

        public async Task<PostViewModel> GetPost(int id)
        {
            using (var context = this.contextFactory.Create())
            {
                return await context.Posts.Where(p => p.Id == id)
                                          .ProjectTo<PostViewModel>()
                                          .FirstOrDefaultAsync();
            }
        }

        public async Task<int> CreatePost(PostViewModel pvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var post = new Post
                {
                    UserId = pvm.UserId,
                    Created = pvm.Created,
                    Title = pvm.Title,
                    Content = pvm.Content,
                    IsActive = pvm.IsActive
                };

                context.Posts.Add(post);

                await context.SaveAsync();

                return post.Id;
            }
        }

        public async Task<int> UpdatePost(PostViewModel pvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == pvm.Id);
                if (post != null)
                {
                    post.UserId = pvm.UserId;
                    post.Created = pvm.Created;
                    post.Title = pvm.Title;
                    post.Content = pvm.Content;
                    post.IsActive = pvm.IsActive;

                    await context.SaveAsync();

                    return post.Id;
                }
                else
                {
                    return pvm.Id;
                }
            }
        }

        public async Task DeletePost(int id)
        {
            using (var context = this.contextFactory.Create())
            {
                var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == id);
                context.Posts.Remove(post);
                await context.SaveAsync();
            }
        }

        public async Task<MediaViewModel> GetMedia(int id, int pid = 0)
        {
            using (var context = this.contextFactory.Create())
            {
                return await context.Medias.Where(p => p.Id == id).ProjectTo<MediaViewModel>().FirstOrDefaultAsync();
            }
        }

        public async Task CreateMedia(MediaViewModel mvm)
        {
            using (var context = this.contextFactory.Create())
            {
                context.Medias.Add(new Media
                {
                    PostId = mvm.PostId,
                    Url = mvm.Url,
                    DataSize = mvm.DataSize,
                    IsPalette = mvm.IsPalette
                });

                await context.SaveAsync();
            }
        }

        public async Task UpdateMedia(MediaViewModel mvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var media = await context.Medias.FirstOrDefaultAsync(p => p.Id == mvm.Id);
                media.PostId = mvm.PostId;
                media.Url = mvm.Url;
                media.DataSize = mvm.DataSize;
                media.IsPalette = mvm.IsPalette;
                await context.SaveAsync();
            }
        }

        public async Task DeleteMeldia(int id)
        {
            using (var context = this.contextFactory.Create())
            {
                var media = await context.Medias.FirstOrDefaultAsync(p => p.Id == id);
                context.Medias.Remove(media);
                await context.SaveAsync();
            }
        }

        public async Task<BlogViewModel> GetBlog(int page)
        {
            using (var context = this.contextFactory.Create())
            {
                var postsCount = await context.Posts.Where(p => p.IsActive == true).CountAsync();
                var pager = new Pager(postsCount, page);
                var pvm = await context.Posts
                                       .Include(p => p.Medias)
                                       .Include(p => p.User)
                                       .Where(p => p.IsActive == true)
                                       .OrderByDescending(p => p.Created)
                                       .Skip((pager.CurrentPage - 1) * pager.PageSize)
                                       .Take(pager.PageSize)
                                       .ProjectTo<PostViewModel>()
                                       .ToListAsync();

                return new BlogViewModel()
                {
                    Pager = pager,
                    Posts = pvm
                };
            }
        }
    }
}
