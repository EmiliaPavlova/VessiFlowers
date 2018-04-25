namespace VessiFlowers.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("vf.Gallery")]
    public partial class Gallery
    {
        public Gallery()
        {
            this.Medias = new HashSet<Media>();
        }

        public int Id { get; set; }

        public int GalleryTypeId { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        public int Position { get; set; }

        public string Content { get; set; }

        public DateTime Created { get; set; }

        [StringLength(256)]
        public string Duration { get; set; }

        [StringLength(256)]
        public string Price { get; set; }

        public virtual GalleryType GalleryType { get; set; }

        public virtual ICollection<Media> Medias { get; set; }
    }
}
