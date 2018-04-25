namespace VessiFlowers.Business.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using VessiFlowers.Business.Models.DomainModels;

    public interface IGalleryService
    {
        Task<GalleryTypeViewModel> GetGalleryType(int id);

        IEnumerable<GalleryTypeViewModel> GetGalleryTypes();

        Task ReorderPosition(GalleryViewModel gvm);

        Task RePositionGalleries(int gtid);

        Task<GalleryViewModel> GetGallery(int id);

        Task<int> CreateGallery(GalleryViewModel gvm);

        Task UpdateGallery(GalleryViewModel gvm);

        Task RemoveGallery(int id);

        Task<MediaViewModel> GetMedia(int id, int gid = 0, bool isPalette = false);

        Task CreateMedia(MediaViewModel mvm);

        Task UpdateMedia(MediaViewModel mvm);

        Task DeleteMeldia(int id);
    }
}
