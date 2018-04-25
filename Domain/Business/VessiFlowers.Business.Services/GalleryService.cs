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
    using VessiFlowers.Business.Models.DomainModels;
    using VessiFlowers.Common.Utilities;
    using VessiFlowers.Data.Entities;
    using VessiFlowers.Data.Repositories;

    public class GalleryService : IGalleryService
    {
        private readonly InstanceFactory<VessiFlowersContext> contextFactory;

        public GalleryService(InstanceFactory<VessiFlowersContext> contextFactory)
        {
            this.contextFactory = contextFactory ?? throw new ArgumentNullException(nameof(contextFactory));
        }

        public async Task<GalleryTypeViewModel> GetGalleryType(int id)
        {
            using (var context = this.contextFactory.Create())
            {
                return await context.GalleryTypes
                                    .Include(gt => gt.Galleries)
                                    .Include(gt => gt.Galleries.Select(g => g.Medias))
                                    .Where(gt => gt.Id == id)
                                    .ProjectTo<GalleryTypeViewModel>()
                                    .FirstOrDefaultAsync();
            }
        }

        public IEnumerable<GalleryTypeViewModel> GetGalleryTypes()
        {
            using (var context = this.contextFactory.Create())
            {
                return context.GalleryTypes.OrderBy(gt => gt.Id).ProjectTo<GalleryTypeViewModel>().ToList();
            }
        }

        public async Task ReorderPosition(GalleryViewModel gvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var galleries = await context.Galleries.Where(gt => gt.Id == gvm.GalleryTypeId).ToArrayAsync();
                int newPossition = gvm.Position;
                for (int i = 0; i < galleries.Length; i++)
                {
                    if (galleries[i].Position == newPossition)
                    {
                        galleries[i].Position = newPossition + 1;
                        newPossition += 1;
                    }
                }
            }
        }

        public async Task RePositionGalleries(int gtid)
        {
            using (var context = this.contextFactory.Create())
            {
                var others = await context.Galleries
                                          .Where(g => g.GalleryTypeId == gtid)
                                          .OrderBy(m => m.Position)
                                          .ToArrayAsync();

                for (int i = 0; i < others.Length; i++)
                {
                    others[i].Position = i + 1;
                }

                await context.SaveAsync();
            }
        }

        public async Task<GalleryViewModel> GetGallery(int id)
        {
            using (var context = this.contextFactory.Create())
            {
                return await context.Galleries.Include(g => g.Medias).Where(g => g.Id == id).ProjectTo<GalleryViewModel>().FirstOrDefaultAsync();
            }
        }

        public async Task<int> CreateGallery(GalleryViewModel gvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var gallery = new Gallery
                {
                    GalleryTypeId = gvm.Id,
                    Position = gvm.Position,
                    Created = gvm.Created,
                    Title = gvm.Title,
                    Content = gvm.Content,
                    Duration = gvm.Duration,
                    Price = gvm.Price
                };

                context.Galleries.Add(gallery);
                await context.SaveAsync();

                var slug = new Slug
                {
                    Url = gallery.Title.Replace(" ", "-"),
                    Controller = "gallery",
                    Action = "details",
                    Param = gallery.Id
                };

                context.Slugs.Add(slug);
                await context.SaveAsync();

                return gallery.Id;
            }
        }

        public async Task UpdateGallery(GalleryViewModel gvm)
        {
            using (var context = this.contextFactory.Create())
            {
                var gallery = await context.Galleries.FirstOrDefaultAsync(g => g.Id == gvm.Id);
                if (gallery != null)
                {
                    await this.ReorderPosition(gvm);

                    gallery.GalleryTypeId = gvm.GalleryTypeId;
                    gallery.Created = gvm.Created;
                    gallery.Title = gvm.Title;
                    gallery.Content = gvm.Content;
                    gallery.Position = gvm.Position;
                    gallery.Duration = gvm.Duration;
                    gallery.Price = gvm.Price;

                    await context.SaveAsync();
                    await this.RePositionGalleries(gallery.GalleryTypeId);
                }

                var slug = await context.Slugs.FirstOrDefaultAsync(s => s.Controller == "gallery" && s.Action == "details" && s.Param == gallery.Id);
                if (slug != null)
                {
                    slug.Url = gallery.Title.Replace(" ", "-");
                    await context.SaveAsync();
                }
                else
                {
                    slug = new Slug
                    {
                        Url = gallery.Title.Replace(" ", "-"),
                        Controller = "gallery",
                        Action = "details",
                        Param = gallery.Id
                    };
                    context.Slugs.Add(slug);
                    await context.SaveAsync();
                }
            }
        }

        public async Task RemoveGallery(int id)
        {
            using (var context = this.contextFactory.Create())
            {
                var gallery = await context.Galleries.FirstOrDefaultAsync(m => m.Id == id);
                if (gallery != null)
                {
                    context.Galleries.Remove(gallery);
                    await context.SaveAsync();
                }
            }
        }

        public async Task<MediaViewModel> GetMedia(int id, int gid = 0, bool isPalette = false)
        {
            using (var context = this.contextFactory.Create())
            {
                var media = new MediaViewModel();
                if (id != 0)
                {
                    media = await context.Medias.ProjectTo<MediaViewModel>().FirstOrDefaultAsync(m => m.Id == id);
                }

                if (gid != 0)
                {
                    media.GalleryId = gid;
                }

                media.IsPalette = isPalette;
                return media;
            }
        }

        public async Task CreateMedia(MediaViewModel mvm)
        {
            using (var context = this.contextFactory.Create())
            {
                context.Medias.Add(new Media
                {
                    GalleryId = mvm.GalleryId,
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
                var existingMedia = await context.Medias.FirstOrDefaultAsync(m => m.Id == mvm.Id);
                if (existingMedia != null)
                {
                    existingMedia.Url = mvm.Url;
                    existingMedia.DataSize = mvm.DataSize;
                    existingMedia.IsPalette = mvm.IsPalette;
                    await context.SaveAsync();
                }
            }
        }

        public async Task DeleteMeldia(int id)
        {
            using (var context = this.contextFactory.Create())
            {
                var existingMedia = await context.Medias.FirstOrDefaultAsync(m => m.Id == id);
                if (existingMedia != null)
                {
                    context.Medias.Remove(existingMedia);
                    await context.SaveAsync();
                }
            }
        }
    }
}