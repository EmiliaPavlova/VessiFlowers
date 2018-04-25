namespace VessiFlowers.Data.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using VessiFlowers.Data.Entities;

    public interface IVessiFlowersContext : IDisposable
    {
        IDbSet<User> Users { get; set; }

        IDbSet<Gallery> Galleries { get; set; }

        IDbSet<GalleryType> GalleryTypes { get; set; }

        IDbSet<Media> Medias { get; set; }

        IDbSet<Page> Pages { get; set; }

        IDbSet<Post> Posts { get; set; }

        IDbSet<Slug> Slugs { get; set; }

        Task<int> SaveAsync();
    }
}
