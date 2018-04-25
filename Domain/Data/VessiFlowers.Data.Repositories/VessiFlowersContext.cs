namespace VessiFlowers.Data.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Microsoft.AspNet.Identity.EntityFramework;
    using VessiFlowers.Data.Entities;

    public partial class VessiFlowersContext : IdentityDbContext<User>, IVessiFlowersContext
    {
        public VessiFlowersContext()
            : base("name=VessiFlowersContext")
        {
        }

        public virtual IDbSet<Gallery> Galleries { get; set; }

        public virtual IDbSet<GalleryType> GalleryTypes { get; set; }

        public virtual IDbSet<Media> Medias { get; set; }

        public virtual IDbSet<Page> Pages { get; set; }

        public virtual IDbSet<Post> Posts { get; set; }

        public virtual IDbSet<Slug> Slugs { get; set; }

        public static VessiFlowersContext Create()
        {
            return new VessiFlowersContext();
        }

        public Task<int> SaveAsync()
        {
            return this.SaveChangesAsync();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gallery>()
                .HasMany(e => e.Medias)
                .WithOptional(e => e.Gallery)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Medias)
                .WithOptional(e => e.Post)
                .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }
    }
}
