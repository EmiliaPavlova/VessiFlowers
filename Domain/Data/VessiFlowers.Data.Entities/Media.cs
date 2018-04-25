namespace VessiFlowers.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vf.Medias")]
    public partial class Media
    {
        public int Id { get; set; }

        public int? GalleryId { get; set; }

        [Required]
        [StringLength(128)]
        public string Url { get; set; }

        [Required]
        [StringLength(16)]
        public string DataSize { get; set; }

        public int? PostId { get; set; }

        public bool IsPalette { get; set; }

        public virtual Gallery Gallery { get; set; }

        public virtual Post Post { get; set; }
    }
}
